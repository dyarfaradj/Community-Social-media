using System;
using System.Collections.Generic;
using System.Text;
using DataAcces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAcces.DataContext
{
    public class DatabaseContext: DbContext
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
            //builder.Entity<Group>()
            //.HasMany(c => c.Members)
            //.WithOne(e => e.Group);
            //builder.Entity<Member>()
            //.HasOne(c => c.Group)
            //.WithMany(e => e.Members);
            builder.Entity<Group>()
                .HasKey(group => group.Id);

            builder.Entity<Member>()
                .HasKey(member => member.Id);

            builder.Entity<Group>()
                .HasMany<Member>(group => group.Members)
                .WithOne(member => member.Group);

            builder.Entity<Member>()
                .HasOne<Group>(member => member.Group)
                .WithMany(group => group.Members);
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
