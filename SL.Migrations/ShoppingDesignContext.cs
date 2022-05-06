using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Migrations
{
    class ShoppingDesignContext : IDesignTimeDbContextFactory<ShoppingContext>
    {
        public ShoppingContext CreateDbContext(string[] args)
        {
            var connString = args.FirstOrDefault() ?? Environment.GetEnvironmentVariable("SLConnString");
            return CreateDbContext(connString);
        }

        public ShoppingContext CreateDbContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"TenantAdmin connection string cannot be null or empty");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ShoppingContext>();

            optionsBuilder
                .UseSqlServer(
                    connectionString,
                    b => {
                        b.MigrationsHistoryTable("__SLMigrationsHistory");
                        b.CommandTimeout(60);
                        b.MigrationsAssembly(typeof(ShoppingDesignContext).Assembly.FullName);
                    })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new ShoppingContext(optionsBuilder.Options);
        }
    }
}
