namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    public class LockoutModel : PageModel
    {
        public void OnGet() { }
    }
}
