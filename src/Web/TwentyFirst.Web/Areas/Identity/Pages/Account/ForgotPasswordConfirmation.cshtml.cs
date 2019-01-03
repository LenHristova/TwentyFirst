namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = GlobalConstants.MasterAdministratorOrAdministrator)]
    public class ForgotPasswordConfirmation : PageModel
    {
        public void OnGet() { }
    }
}
