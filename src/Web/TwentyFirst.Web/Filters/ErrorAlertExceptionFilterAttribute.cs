namespace TwentyFirst.Web.Filters
{
    using Common.Constants;
    using Common.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;

    public class ErrorAlertExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IModelMetadataProvider modelMetadataProvider;
        private readonly ILogger logger;

        public ErrorAlertExceptionFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider, 
            ILogger<ErrorAlertExceptionFilterAttribute> logger)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.modelMetadataProvider = modelMetadataProvider;
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            this.logger.LogError(context.Exception.Message);

            var result = new PartialViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState)
            };

            if (!hostingEnvironment.IsDevelopment())
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var errorMessage = context.Exception is BaseTwentyFirstException ex
                        ? ex.Message
                        : GlobalConstants.BaseExceptionMessage;

                    result.ViewData.Add("Error", errorMessage);
                    context.Result = result;
                }
                else
                {
                    context.Result = new EmptyResult();
                }
            }
            else
            {
                result.ViewData.Add("Error", context.Exception);
                context.Result = result;
            }
        }
    }
}
