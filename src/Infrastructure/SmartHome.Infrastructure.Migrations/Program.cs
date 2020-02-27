using System;
using Microsoft.EntityFrameworkCore;
using SmartHome.Infrastructure.Configuration;
using SmartHome.Infrastructure.Persistence;

namespace SmartHome.Infrastructure.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Applying migrations...");
            var configProvider = new ConfigProvider();
            using var dbContext = (new DbContextFactory()).CreateDbContext(new string[]{ });
            dbContext.Database.Migrate();
            Console.WriteLine("Migration applied successfully");
        }

        
    }
}
