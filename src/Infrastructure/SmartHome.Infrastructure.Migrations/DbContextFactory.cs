using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SmartHome.Infrastructure.Persistence;

namespace SmartHome.Infrastructure.Migrations
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public static readonly string[] EmptyArgs = new string[0] { };
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(
                configuration.GetConnectionString("dbConnectionString"),
                x =>
                {
                    x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    x.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                });

            return new ApplicationDbContext(builder.Options);
        }
    }
}
