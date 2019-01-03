namespace TwentyFirst.Web.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize]
    public class AccessDeniedModel : PageModel
    {
        public void OnGet() { }
    }
}

