using System;
using System.Collections.Generic;
using System.Text;
using DataAcces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAcces.DataContext
{
    public class DatabaseContext: IdentityDbContext<User>
    {
        public class OptionsBuild
        {
            public OptionsBuild()
            {
                settings = new AppConfiguration();
                opsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                opsBuilder.UseSqlServer(settings.sqlConnectionString);
                dbOptions = opsBuilder.Options;
            }
            public DbContextOptionsBuilder<DatabaseContext> opsBuilder { get; set; }
            public DbContextOptions<DatabaseContext> dbOptions { get; set; }

            private AppConfiguration settings { get; set; }
        }
        public static OptionsBuild ops = new OptionsBuild();

        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
