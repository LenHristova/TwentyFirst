namespace TwentyFirst.Web.Controllers
{
    using Common.Extensions;
    using Common.Models.Enums;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        /// <summary>
        /// Set alert message to TempData.
        /// Only last added message is available.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        protected void SetAlertMessage(AlertMessageLevel level, string message)
        {
            this.TempData["AlertLevelColor"] = level.GetDisplayName();
            this.TempData["AlertMessage"] = message;
        }
    }
}
