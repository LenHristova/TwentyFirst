namespace TwentyFirst.Web.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Exceptions;
    using Logging;
    using Microsoft.AspNetCore.Authorization;
    using TwentyFirst.Data.Models;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<ChangePasswordModel> logger;

        public ChangePasswordModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [DataType(DataType.Password)]
            [Display(Name = "Настояща парола")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [MinLength(GlobalConstants.MinPasswordLength, ErrorMessage = ValidationErrorMessages.MinLength)]
            [MaxLength(GlobalConstants.MaxPasswordLength, ErrorMessage = ValidationErrorMessages.MaxLength)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърждение на парола")]
            [Compare("NewPassword", ErrorMessage = ValidationErrorMessages.PasswordConfirmation)]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidAccountIdException(userManager.GetUserId(User));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidAccountIdException(userManager.GetUserId(User));
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await signInManager.RefreshSignInAsync(user);
            logger.LogInformation((int)LoggingEvents.UpdateItem, "Паролата на главния администратор беше променена успешно.");
            StatusMessage = "Паролата ви беше променена.";

            return RedirectToPage();
        }
    }
}
