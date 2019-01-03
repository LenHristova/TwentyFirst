using Microsoft.AspNetCore.Hosting;
using TwentyFirst.Web.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace TwentyFirst.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}