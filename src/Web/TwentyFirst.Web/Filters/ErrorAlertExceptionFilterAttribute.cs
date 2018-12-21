namespace TwentyFirst.Web.Filters
{
    using Common.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class ErrorAlertExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IModelMetadataProvider modelMetadataProvider;

        public ErrorAlertExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            var result = new PartialViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState)
            };

            if (!hostingEnvironment.IsDevelopment())
            {
                if (context.Exception is BaseTwentyFirstException &&
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
    }
}
