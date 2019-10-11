using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disribuerade_System_Labb_2.Models;
using Distibuerade_System_Labb_2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Distibuerade_System_Labb_2.Models
{
    public class Distribuerade_System_Labb_2Context : IdentityDbContext<Distibuerade_System_Labb_2User>
    {
        public Distribuerade_System_Labb_2Context(DbContextOptions<Distribuerade_System_Labb_2Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Message> Messages { get; set; }
    }
}
