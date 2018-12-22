namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Exceptions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using TwentyFirst.Data.Models;

    public class ConfirmEmailModel : IdentityAdministrationBasePageModel<ConfirmEmailModel>
    {
        private readonly UserManager<User> userManager;

        public ConfirmEmailModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
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

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Възникна грешка при потвърждаването по емайл на акаунт с ID \"{userId}\":");
            }

            return Page();
        }
    }
}
