using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(de_exceptional_closures.Areas.Identity.IdentityHostingStartup))]
namespace de_exceptional_closures.Areas.Identity
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