namespace TwentyFirst.Services.DataServices.Tests
{
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class LogServiceTests : DataServiceTests
    {
        private readonly LogService logService;

        public LogServiceTests()
        {
            this.logService = new LogService(this.dbContext);
        }

        [Fact]
        public async Task LogAsync_AddNewLogToDb()
        {
            await this.logService.LogAsync("Log", new EventId(), LogLevel.Error);
            const int expected = 1;
            var actual = this.dbContext.Logs.Count();

            Assert.Equal(expected, actual);
        }
    }
}
