namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = GlobalConstants.MasterAdministratorOrAdministrator)]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public ForgotPasswordModel(
            UserManager<User> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [Display(Name = "Потребителско име")]
            public string Username { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Input.Username);

                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user);

                await this.SendConfirmationEmail(
                    user.Id,
                    code,
                    callBackPageName: "/Account/ResetPassword",
                    subject: "Възстановяване на парола",
                    content: $"Моля възстановете паролата на акаунт \"{user.UserName}\"");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        private async Task SendConfirmationEmail(string userId, string code, string callBackPageName, string subject, string content)
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
