namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using TwentyFirst.Data.Models;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public RegisterModel(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [MinLength(6, ErrorMessage = ValidationErrorMessages.MinLength)]
            [MaxLength(50, ErrorMessage = ValidationErrorMessages.MaxLength)]
            [Display(Name = "Потребителско име")]
            public string Username { get; set; }

            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [MinLength(6, ErrorMessage = ValidationErrorMessages.MinLength)]
            [MaxLength(100, ErrorMessage = ValidationErrorMessages.MaxLength)]
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
            returnUrl = returnUrl ?? Url.Content(GlobalConstants.AdministrationHomePage);
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.Username };
                var result = await userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.EditorRoleName);
                    logger.LogInformation((int)LoggingEvents.InsertItem, $"Беше регистриран нов акаунт с ID \"{user.Id}\".");

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    var masterAdminUsername = this.configuration["MasterAdministratorAccount:Username"];
                    var masterAdminUser = await this.userManager.Users.SingleOrDefaultAsync(u => u.UserName == masterAdminUsername);

                    await emailSender.SendEmailAsync(masterAdminUser.Email, "Потвърждение на новорегистриран акаунт",
                        $"Моля потвърдете новорегистрирания акаунт с username: \"{user.UserName}\" <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>кликайки тук</a>.");
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
