namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.Exceptions;
    using Data.Models;
    using Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class UnlockModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<UnlockModel> logger;

        public UnlockModel(UserManager<User> userManager, ILogger<UnlockModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public UserToUnlockModel UserToUnlock { get; set; }

        public class UserToUnlockModel
        {
            public string Id { get; set; }

            public string Username { get; set; }
        }

        public async Task<IActionResult> OnGet(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidAccountIdException(userId);
            }

            UserToUnlock = new UserToUnlockModel
            {
                Id = userId,
                Username = user.UserName
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage("./AccessDenied");
            }

            var result = await this.userManager.SetLockoutEndDateAsync(user, null);

            var message = $"Акаунтът на {user.UserName} беше отключен.";
            this.logger.LogInformation((int)LoggingEvents.UpdateItem, message);

            if (result.Succeeded)
            {
                return RedirectToPage("./UnlockConfirmation", new { userId });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
