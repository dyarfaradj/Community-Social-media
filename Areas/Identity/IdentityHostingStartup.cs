using System;
using Distibuerade_System_Labb_2.Areas.Identity.Data;
using Distibuerade_System_Labb_2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Distibuerade_System_Labb_2.Areas.Identity.IdentityHostingStartup))]
namespace Distibuerade_System_Labb_2.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDefaultIdentity<Distibuerade_System_Labb_2User>()
                    .AddEntityFrameworkStores<Distribuerade_System_Labb_2Context>();
            });
        }
    }
}