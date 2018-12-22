namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TwentyFirst.Data.Models;

    public class ForgotPasswordModel : IdentityAdministrationEmailConfirmationPageModel<ForgotPasswordModel>
    {
        public ForgotPasswordModel(
            UserManager<User> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
            : base(userManager, emailSender, configuration)
        { }

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
    }
}
