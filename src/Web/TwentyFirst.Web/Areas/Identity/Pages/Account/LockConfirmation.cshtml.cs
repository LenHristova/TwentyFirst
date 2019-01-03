namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class LockConfirmationModel : PageModel
    {
        private readonly UserManager<User> userManager;

        public LockConfirmationModel(UserManager<User> userManager)
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
