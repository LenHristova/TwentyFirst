namespace TwentyFirst.Web.Areas.Identity
{
    using Common.Constants;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public abstract class AdministrationEmailConfirmationPageModel<TPageModel> : AdministrationPageModel<TPageModel>
    {
        protected readonly UserManager<User> userManager;
        protected readonly IEmailSender emailSender;
        protected readonly IConfiguration configuration;

        protected AdministrationEmailConfirmationPageModel(
            UserManager<User> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        protected async Task SendConfirmationEmail(string userId, string code, string callBackPageName, string subject, string content)
        {
            var callbackUrl = Url.Page(
                pageName: callBackPageName,
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            var masterAdminUsername = this.configuration[GlobalConstants.MasterAdministratorUsernameConfiguration];
            var masterAdminUser = await this.userManager.FindByNameAsync(masterAdminUsername);

            await emailSender.SendEmailAsync(
                masterAdminUser.Email,
                subject,
                content + $" <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>кликайки тук</a>.");
        }
    }
}
