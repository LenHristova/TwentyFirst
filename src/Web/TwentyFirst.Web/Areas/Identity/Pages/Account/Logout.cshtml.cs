namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;
    using Common.Constants;
    using Data.Models;

    [Authorize]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<LogoutModel> logger;

        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGet() => await this.OnPost();

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            returnUrl = returnUrl ?? Url.Content(GlobalConstants.AdministrationLoginPage);
            return LocalRedirect(returnUrl);
        }
    }
}