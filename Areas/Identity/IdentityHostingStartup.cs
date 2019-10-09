using System;
using Distribuerade_System_Labb_2.Areas.Identity.Data;
using Distribuerade_System_Labb_2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Distribuerade_System_Labb_2.Areas.Identity.IdentityHostingStartup))]
namespace Distribuerade_System_Labb_2.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Distribuerade_System_Labb_2Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Distribuerade_System_Labb_2ContextConnection")));

                services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<Distribuerade_System_Labb_2Context>();
            });
        }
    }
}