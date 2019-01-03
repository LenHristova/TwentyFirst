namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class UnlockConfirmationModel : PageModel
    {
        private readonly UserManager<User> userManager;

        public UnlockConfirmationModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public string UsernameForUserPasswordReset { get; set; }

        public async Task<IActionResult> OnGet(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage("./AccessDenied");
            }

            UsernameForUserPasswordReset = user.UserName;
            return Page();
        }
    }
}
