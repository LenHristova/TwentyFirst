namespace TwentyFirst.Web.Logging
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Services.DataServices.Contracts;
    using System;

    public class DbLogger : ILogger
    {
        private readonly string categoryName;
        private readonly Func<string, LogLevel, bool> filter;
        private readonly IApplicationBuilder app;
        private const int MessageMaxLength = 4000;

        public DbLogger(
            string categoryName,
            Func<string, LogLevel, bool> filter,
            IApplicationBuilder app)
        {
            this.categoryName = categoryName;
            this.filter = filter;
            this.app = app;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) && !IsEnabled(eventId.Id))
            {
                return;
            }
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (exception != null)
            {
                message += "\n" + exception;
            }
           
            try
            {
                message = message.Length > MessageMaxLength ? message.Substring(0, MessageMaxLength) : message;

                var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                var scope = serviceFactory.CreateScope();
                using (scope)
                {
                    var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
                    logService.LogAsync(message, eventId, logLevel).GetAwaiter().GetResult();
                }
            }
            catch
            {
                // ignored
            }
        }

        public bool IsEnabled(LogLevel logLevel)
            => (filter == null || filter(categoryName, logLevel));

        public bool IsEnabled(int eventId)
              => Enum.IsDefined(typeof(LoggingEvents), eventId);

        public IDisposable BeginScope<TState>(TState state)
            => null;
    }
}
