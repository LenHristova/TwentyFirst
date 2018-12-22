namespace TwentyFirst.Web.Areas.Identity
{
    using Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = GlobalConstants.MasterAdministratorOrAdministrator)]
    //[Authorize(Roles = GlobalConstants.MasterAdministratorRoleName)]
    //[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public abstract class IdentityAdministrationBasePageModel<TPageModel> : PageModel
    {
    }
}
