using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kooboo.Mail
{
    public class DesignTimeDbContextFactory:
        IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            var options = new DbContextOptionsBuilder()
                .UseSqlite(config.GetConnectionString("Main"))
                .Options;

            return new MyDbContext(options);
        }
    }
}
