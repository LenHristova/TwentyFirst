namespace TwentyFirst.Web.Areas.Identity
{
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = GlobalConstants.MasterAdministratorOrAdministrator)]
    public abstract class AdministrationPageModel<TPageModel> : PageModel
    {
    }
}
