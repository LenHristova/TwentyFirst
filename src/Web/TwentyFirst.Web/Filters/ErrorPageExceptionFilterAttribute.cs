namespace TwentyFirst.Web.Filters
{
    using Common.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class ErrorPageExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IModelMetadataProvider modelMetadataProvider;

        public ErrorPageExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            var result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState)
            };

            if (!hostingEnvironment.IsDevelopment())
            {
                if (context.Exception is ITwentyFirstException &&
                    context.HttpContext.User.Identity.IsAuthenticated)
                {
                    result.ViewData.Add("Error", context.Exception.Message);
                    context.Result = result;
                }

                return;
            }

            result.ViewData.Add("Error", context.Exception);
            context.Result = result;
        }

        //public void OnException(ExceptionContext context)
        //{
        //    var tempData = tempDataFactory.GetTempData(context.HttpContext);

        //    if (context.Exception is ITwentyFirstException &&
        //        context.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        tempData["AlertLevelColor"] = AlertMessageLevel.Error.GetDisplayName();
        //        tempData["AlertMessage"] = context.Exception.Message;
        //        context.Result = new PartialViewResult(){ViewName = "_AlertMessagePartial"};
        //        //context.Result = new RedirectResult("/Administration/Articles");
        //        //context.Result = new RedirectResult("/Home/Error");

        //        //context.Result = new RedirectResult("/Home/Error");
        //        //context.Result = new ViewResult();
        //    }
        //    else
        //    {
        //        context.Result = new RedirectResult("/");
        //    }
        //}
    }
}
