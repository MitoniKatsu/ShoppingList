using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace SL.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();

            var connectionString = config.GetValue<string>("SLConnString");
            var factory = new ShoppingDesignContext();
            var context = factory.CreateDbContext(connectionString);

            var db = context.Database;

            if (db == null)
            {
                Console.WriteLine("Could not create dbContext");
                return;
            }

            Console.WriteLine($"Provider: {db.ProviderName}");

            Console.WriteLine("Migrations:");

            foreach (var m in db.GetMigrations())
            {
                Console.WriteLine($"\t{m}");
            }

            var pending = db.GetPendingMigrations();

            Console.WriteLine("Pending:");
            foreach (var m in db.GetPendingMigrations())
            {
                Console.WriteLine($"\t{m}");
            }

            if (!pending.Any())
            {
                Console.WriteLine("No migrations");
                return;
            }

            db.Migrate();
            Console.WriteLine("Complete");
        }
    }
}
