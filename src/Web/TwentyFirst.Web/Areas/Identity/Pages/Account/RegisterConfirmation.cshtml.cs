namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;

    [Authorize(Roles = GlobalConstants.MasterAdministratorOrAdministrator)]
    public class RegisterConfirmation
    {
        public void OnGet() { }
    }
}
