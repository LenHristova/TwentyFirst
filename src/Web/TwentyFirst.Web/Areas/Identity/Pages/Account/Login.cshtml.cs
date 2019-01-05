namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Common.Constants;
    using Data.Models;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<LoginModel> logger;

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [Display(Name = "Потребителско име")]
            public string Username { get; set; }

            [Required(ErrorMessage = ValidationErrorMessages.Required)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content(GlobalConstants.AdministrationHomePage);

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content(GlobalConstants.AdministrationHomePage);

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(Input.Username, Input.Password, false, true);

                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl });
                }
                if (result.IsLockedOut)
                {
                    logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError("Input.Username", ValidationErrorMessages.InvalidCredentials);
                    ModelState.AddModelError("Input.Password", ValidationErrorMessages.InvalidCredentials);
                    return Page();
                }
            }

            return Page();
        }
    }
}
