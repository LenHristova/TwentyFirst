namespace TwentyFirst.Web.Filters
{
    using Common.Constants;
    using Common.Exceptions;
    using Common.Extensions;
    using Common.Models.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ITempDataDictionaryFactory tempDataFactory;

        public GlobalExceptionFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            this.tempDataFactory = tempDataFactory;
        }

        public void OnException(ExceptionContext context)
        {
            var tempData = tempDataFactory.GetTempData(context.HttpContext);

            if (context.Exception is ITwentyFirstException &&
                context.HttpContext.User.Identity.IsAuthenticated)
            {
                tempData["AlertLevelColor"] = AlertMessageLevel.Error.GetDisplayName();
                tempData["AlertMessage"] = context.Exception.Message;
                context.Result = new RedirectResult(GlobalConstants.AdministrationHomePage);
            }
            else
            {
                context.Result = new RedirectResult("/");
            }
        }
    }
}
