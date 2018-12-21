namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public interface ILogService
    {
        Task LogAsync(string message, EventId eventId, LogLevel logLevel);
    }
}
