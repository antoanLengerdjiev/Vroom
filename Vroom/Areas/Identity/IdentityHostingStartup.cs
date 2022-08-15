using System;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Vroom.Areas.Identity.IdentityHostingStartup))]
namespace Vroom.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}