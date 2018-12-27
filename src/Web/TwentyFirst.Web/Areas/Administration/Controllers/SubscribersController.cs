namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using System;
    using Common.Constants;
    using Common.Models.Articles;
    using Common.Models.Subscribers;
    using Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Services.DataServices.Contracts;
    using System.IO;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    [TypeFilter(typeof(ErrorPageExceptionFilterAttribute))]
    public class SubscribersController : AdministrationController
    {
        private readonly ISubscriberService subscriberService;
        private readonly IArticleService articleService;
        private readonly IEmailSender emailSender;
        private readonly LinkGenerator linkGenerator;
        private readonly IHostingEnvironment env;

        public SubscribersController(
            ISubscriberService subscriberService,
            IArticleService articleService,
            IEmailSender emailSender,
            LinkGenerator linkGenerator,
            IHostingEnvironment env)
        {
            this.subscriberService = subscriberService;
            this.articleService = articleService;
            this.emailSender = emailSender;
            this.linkGenerator = linkGenerator;
            this.env = env;
        }

        public IActionResult ConfirmArticlesSend() => this.View();

        public async Task<IActionResult> SendImportantArticles()
        {
            var articlesToSend = await this.articleService.AllImportantForTheDayAsync<ArticleBaseViewModel>();

            var stringBuilder = new StringBuilder();
            var singleArticleFilePath = this.GetHtmlTemplateFilePath(GlobalConstants.HtmlSingleArticleFilePath);
            var singleArticleHtml = await System.IO.File.ReadAllTextAsync(singleArticleFilePath);
            
            foreach (var article in articlesToSend)
            {
                var articleUrl = this.linkGenerator.GetUriByAction(
                    this.HttpContext,
                    controller: "Articles",
                    action: "Details",
                    values: new { id = article.Id });

                var encodedArticleUrl = HtmlEncoder.Default.Encode(articleUrl);

                stringBuilder.AppendLine(singleArticleHtml
                    .Replace(GlobalConstants.HtmlArticleLinkPlaceholder, encodedArticleUrl)
                    .Replace(GlobalConstants.HtmlArticleTilePlaceholder, article.Title));
            }

            var articlesToEmailFilePath = this.GetHtmlTemplateFilePath(GlobalConstants.HtmlArticlesToEmailFilePath);

            var emailContent = await System.IO.File.ReadAllTextAsync(articlesToEmailFilePath);
            emailContent = emailContent
                .Replace(GlobalConstants.HtmlImportantArticlesPlaceholder, stringBuilder.ToString().Trim());

            var subscribers = await this.subscriberService.AllConfirmedAsync<SubscriberSendArticlesModel>();
            foreach (var subscriber in subscribers)
            {
                var unsubscribeUrl = this.linkGenerator.GetUriByAction(
                    this.HttpContext,
                    action: "Unsubscribe",
                    values: new { id = subscriber.Id, cc = subscriber.ConfirmationCode });

                var encodedUnsubscribeUrl = HtmlEncoder.Default.Encode(unsubscribeUrl);

                emailContent = emailContent
                    .Replace(GlobalConstants.HtmlUnsubscribeLinkPlaceholder, encodedUnsubscribeUrl);

                await this.emailSender.SendEmailAsync(subscriber.Email, GlobalConstants.ImportantArticlesEmailSubject, emailContent);
            }

            return this.RedirectToAction(nameof(SuccessfulArticlesSend));
        }

        public IActionResult SuccessfulArticlesSend() => this.View();

        private string GetHtmlTemplateFilePath(string templateFileName)
        {
            return env.WebRootPath
                   + Path.DirectorySeparatorChar
                   + GlobalConstants.HtmlTemplatesFolderPath
                   + Path.DirectorySeparatorChar
                   + templateFileName;
        }
    }
}
