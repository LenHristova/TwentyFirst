namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        public IActionResult OnGet()
        {
            if (!this.User.IsInRole(GlobalConstants.MasterAdministratorOrAdministrator))
            {
                return RedirectToPage("./AccessDenied");
            }

            return this.Page();
        }
    }
}
