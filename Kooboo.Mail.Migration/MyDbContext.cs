using System;
using Microsoft.EntityFrameworkCore;

namespace Kooboo.Mail
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options)
          : base(options)
        {
        }

        public DbSet<DbHistory> DbHistories { get; set; }

        public DbSet<EmailAddress> EmailAddresses { get; set; }

        public DbSet<Folder> Folders { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<TargetAddress> TargetAddresses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureModels(builder);

            ConfigureConventions(builder);
        }

        private void ConfigureModels(ModelBuilder builder)
        {
            var dbHistory = builder.Entity<DbHistory>();
            dbHistory.ToTable("__DbHistory");

            var emailAddress = builder.Entity<EmailAddress>();
            emailAddress.ToTable("EmailAddress");

            var folder = builder.Entity<Folder>();
            folder.ToTable("Folder");

            var message = builder.Entity<Message>();
            message.ToTable("Message").Property("Id").ValueGeneratedOnAdd();

            var targetAddress = builder.Entity<TargetAddress>();
            targetAddress.ToTable("TargetAddress");
        }

        private void ConfigureConventions(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties();
                foreach (var property in properties)
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetColumnType("TEXT COLLATE NOCASE");
                    }
                }
            }
        }
    }
}