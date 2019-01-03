namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Data.Models;
    using Logging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class LockModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<LockModel> logger;

        public LockModel(UserManager<User> userManager, ILogger<LockModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public UserToLockModel UserToLock { get; set; }

        public class UserToLockModel
        {
            public string Id { get; set; }

            public string Username { get; set; }
        }

        public async Task<IActionResult> OnGet(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage("./AccessDenied");
            }

            UserToLock = new UserToLockModel
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

            var result = await this.userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue);

            var message = $"Акаунтът на {user.UserName} беше заключен.";
            this.logger.LogInformation((int)LoggingEvents.UpdateItem, message);

            if (result.Succeeded)
            {
                return RedirectToPage("./LockConfirmation", new { userId });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
