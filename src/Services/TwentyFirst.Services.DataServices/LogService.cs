namespace TwentyFirst.Services.DataServices
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.Extensions.Logging;

    public class LogService : ILogService
    {
        private readonly TwentyFirstDbContext db;

        public LogService(TwentyFirstDbContext db)
        {
            this.db = db;
        }

        public async Task LogAsync(string message, EventId eventId, LogLevel logLevel)
        {
            var log = new Log
            {
                Message = message,
                EventId = eventId.Id,
                LogLevel = logLevel.ToString(),
                CreatedTime = DateTime.UtcNow
            };

            await db.Logs.AddAsync(log);

            await db.SaveChangesAsync();
        }
    }
}
