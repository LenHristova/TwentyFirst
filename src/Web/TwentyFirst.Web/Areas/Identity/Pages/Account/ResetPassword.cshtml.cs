namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Common.Exceptions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using TwentyFirst.Data.Models;

    public class ResetPasswordModel : IdentityAdministrationBasePageModel<ResetPasswordModel>
    {
        private readonly UserManager<User> userManager;

        public ResetPasswordModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public string UsernameForUserPasswordReset { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [MinLength(GlobalConstants.MinPasswordLength, ErrorMessage = ValidationErrorMessages.MinLength)]
            [MaxLength(GlobalConstants.MaxPasswordLength, ErrorMessage = ValidationErrorMessages.MaxLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърждение на парола")]
            [Compare("Password", ErrorMessage = ValidationErrorMessages.PasswordConfirmation)]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }

            public string Id { get; set; }
        }

        public async Task<IActionResult> OnGet(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidAccountIdException(userId);
            }

            UsernameForUserPasswordReset = user.UserName;
            Input = new InputModel { Id = userId, Code = code };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.FindByIdAsync(Input.Id);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
