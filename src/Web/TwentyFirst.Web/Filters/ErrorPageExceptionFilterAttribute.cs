namespace TwentyFirst.Web.Filters
{
    using Common.Exceptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using Common.Constants;

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
            if (HasAnotherExceptionFilter(context))
            {
                return;
            }

            this.logger.LogError(context.Exception.Message);

            var result = new ViewResult
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
                    context.Result = new RedirectResult("/");
                }
            }
            else
            {
                result.ViewData.Add("Error", context.Exception);
                context.Result = result;
            }
        }

        private bool HasAnotherExceptionFilter(ExceptionContext context)
        {
            var exceptionFiltersCount = context.ActionDescriptor.FilterDescriptors
                .Select(attr => attr.Filter)
                .Where(f => f is TypeFilterAttribute || f is ExceptionFilterAttribute)
                .Select(f => f is TypeFilterAttribute tf ? tf.ImplementationType : f.GetType())
                .Count(a => a.BaseType == typeof(ExceptionFilterAttribute));

            return exceptionFiltersCount > 1;
        }
    }
}
