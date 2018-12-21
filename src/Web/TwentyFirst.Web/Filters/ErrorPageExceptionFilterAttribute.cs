namespace TwentyFirst.Web.Filters
{
    using Common.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;

    public class ErrorPageExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IModelMetadataProvider modelMetadataProvider;
        private readonly ILogger logger;

        public ErrorPageExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider,
            ILogger<ErrorPageExceptionFilterAttribute> logger)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.modelMetadataProvider = modelMetadataProvider;
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            this.logger.LogError(context.Exception.Message);

            var result = new ViewResult
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
