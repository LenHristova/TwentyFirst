namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Web.Controllers;

    [Area("Administration")]
    [Authorize]
    public abstract class AdministrationController : BaseController
    {
    }
}
