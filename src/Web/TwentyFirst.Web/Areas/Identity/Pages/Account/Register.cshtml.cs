namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Logging;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using TwentyFirst.Data.Models;

    public class RegisterModel : AdministrationEmailConfirmationPageModel<RegisterModel>
    {
        private readonly ILogger<RegisterModel> logger;

        public RegisterModel(
            UserManager<User> userManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration)
            : base(userManager, emailSender, configuration)
        {
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [MinLength(GlobalConstants.MinUsernameLength, ErrorMessage = ValidationErrorMessages.MinLength)]
            [MaxLength(GlobalConstants.MaxUsernameLength, ErrorMessage = ValidationErrorMessages.MaxLength)]
            [Display(Name = "Потребителско име")]
            public string Username { get; set; }

            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [MinLength(GlobalConstants.MinPasswordLength, ErrorMessage = ValidationErrorMessages.MinLength)]
            [MaxLength(GlobalConstants.MaxPasswordLength, ErrorMessage = ValidationErrorMessages.MaxLength)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърждение на парола")]
            [Compare("Password", ErrorMessage = ValidationErrorMessages.PasswordConfirmation)]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content(GlobalConstants.AdministrationAccountsPage);
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.Username };
                var result = await userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.EditorRoleName);
                    logger.LogInformation((int)LoggingEvents.InsertItem, $"Беше регистриран нов акаунт с ID \"{user.Id}\".");

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    await this.SendConfirmationEmail(
                        user.Id,
                        code,
                        callBackPageName: "/Account/ConfirmEmail",
                        subject: "Потвърждение на новорегистриран акаунт",
                        content: $"Моля потвърдете новорегистрирания акаунт с username: \"{user.UserName}\"");

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
