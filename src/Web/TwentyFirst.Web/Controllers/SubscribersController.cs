namespace TwentyFirst.Web.Controllers
{
    using Common.Constants;
    using Common.Models.Subscribers;
    using Filters;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using reCAPTCHA.AspNetCore;
    using Services.DataServices.Contracts;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class SubscribersController : BaseController
    {
        private readonly IEmailSender emailSender;
        private readonly ISubscriberService subscriberService;
        private readonly LinkGenerator linkGenerator;
        private readonly IRecaptchaService recaptcha;
        private readonly IHostingEnvironment env;

        public SubscribersController(
            IEmailSender emailSender,
            ISubscriberService subscriberService,
            LinkGenerator linkGenerator,
            IRecaptchaService recaptcha,
            IHostingEnvironment env)
        {
            this.emailSender = emailSender;
            this.subscriberService = subscriberService;
            this.linkGenerator = linkGenerator;
            this.recaptcha = recaptcha;
            this.env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(
           [Required, MaxLength(300), EmailAddress] string email)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Content(this.GetModelStateErrorMessages());
            }

            var subscriberExists = await this.subscriberService.ExistsAsync(email);

            var subscriber = subscriberExists
                ? await this.subscriberService.GetAsync(email)
                : await this.subscriberService.CreateAsync(email);

            if (subscriber.IsConfirmed)
            {
                return this.Content(GlobalConstants.AlreadySubscribedEmail);
            }

            var pathToFile = env.WebRootPath
                             + Path.DirectorySeparatorChar
                             + GlobalConstants.HtmlTemplatesFolderPath
                             + Path.DirectorySeparatorChar
                             + GlobalConstants.HtmlConfirmationEmailFilePath;

            var emailContent = await System.IO.File.ReadAllTextAsync(pathToFile);

            var url = this.linkGenerator.GetUriByAction(
                this.HttpContext,
                action: "Confirm",
                values: new { id = subscriber.Id, cc = subscriber.ConfirmationCode });

            var callbackUrl = HtmlEncoder.Default.Encode(url);
            emailContent = emailContent.Replace(GlobalConstants.HtmlConfirmationLinkPlaceholder, callbackUrl);

            await this.emailSender.SendEmailAsync(email, GlobalConstants.SubscribeConfirmationEmailSubject, emailContent);

            return this.Content(GlobalConstants.ConfirmationEmailSend);
        }

        public async Task<IActionResult> Confirm(string id, string cc)
        {
            var subscriber = await this.subscriberService.GetAsync<SubscriberModel>(id, cc);
            return this.View(subscriber);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(SubscriberModel subscriberModel)
        {
            if (env.IsDevelopment())
            {
                this.Request.Host = new HostString("localhost");
            }

            var recaptchaResult = await this.recaptcha.Validate(Request);

            if (!recaptchaResult.success)
            {
                this.ModelState.AddModelError("Recaptcha", "Моля потвърдете, че не сте робот!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(subscriberModel);
            }

            await this.subscriberService.SubscribeAsync(subscriberModel.Id, subscriberModel.ConfirmationCode);
            return this.RedirectToAction(nameof(Confirmed));
        }

        public IActionResult Confirmed() => this.View();

        public async Task<IActionResult> Unsubscribe(string id, string cc)
        {
            await this.subscriberService.UnsubscribeAsync(id, cc);
            return this.RedirectToAction(nameof(Unsubscribed));
        }

        public IActionResult Unsubscribed() => this.View();
    }
}
