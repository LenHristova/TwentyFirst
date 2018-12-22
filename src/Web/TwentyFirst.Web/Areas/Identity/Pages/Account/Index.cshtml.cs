namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Data.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Models.Enums;
    using Logging;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using X.PagedList;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IndexModel> logger;

        public IndexModel(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<IndexModel> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public UsersListWrapperViewModel UsersListWrapper { get; set; }

        public IEnumerable<string> AllRoles { get; set; }

        public class UsersListWrapperViewModel
        {
            public IPagedList<UsersListViewModel> Users { get; set; }
        }

        public class UsersListViewModel
        {
            public string Id { get; set; }

            public string Username { get; set; }

            public string Role { get; set; }

            public bool IsLocked { get; set; }
        }

        public async Task OnGet(int? pageNumber)
        {
            var users = new List<UsersListViewModel>();
            foreach (var user in this.userManager.Users)
            {
                if (user.UserName == this.User.Identity.Name)
                {
                    continue;
                }

                var role = await this.userManager.GetRolesAsync(user);
                var isLocked = await this.userManager.IsLockedOutAsync(user);

                var userModel = new UsersListViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Role = role.FirstOrDefault(),
                    IsLocked = isLocked
                };

                users.Add(userModel);
            }

            var onePageOfUsers = await users
                .OrderByDescending(u => u.Role == GlobalConstants.AdministratorRoleName)
                .ThenBy(u => u.IsLocked)
                .ToList()
                .PaginateAsync(pageNumber, GlobalConstants.AdministrationUsersOnPageCount);

            this.UsersListWrapper = new UsersListWrapperViewModel { Users = onePageOfUsers };
            this.AllRoles = this.roleManager.Roles
                .Where(r => r.Name != GlobalConstants.MasterAdministratorRoleName)
                .Select(r => r.Name)
                .ToList();
        }

        public async Task<IActionResult> OnPost(string userId, string currentRole, string newRole)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var currentRoleExists = await this.roleManager.RoleExistsAsync(currentRole);
            var newRoleExists = await this.roleManager.RoleExistsAsync(currentRole);

            if (user != null && currentRoleExists && newRoleExists)
            {
                await this.userManager.RemoveFromRoleAsync(user, currentRole);
                await this.userManager.AddToRoleAsync(user, newRole);

                var message = $"Ролята на {user.UserName} беше променена от \"{currentRole}\" на \"{newRole}\".";
                this.logger.LogInformation((int)LoggingEvents.UpdateItem, message);

                this.TempData["AlertLevelColor"] = AlertMessageLevel.Success.GetDisplayName();
                this.TempData["AlertMessage"] = message;
            }
            else
            {
                this.TempData["AlertLevelColor"] = AlertMessageLevel.Error.GetDisplayName();
                this.TempData["AlertMessage"] = "Нещо се обърка.";
            }

            return RedirectToPage("./Index");
        }
    }
}