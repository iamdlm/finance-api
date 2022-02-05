using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Fin.Infrastructure.Context
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<FinDbContext>
    {
        public FinDbContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Prepare configuration builder
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: false)
                .Build();

            // Create DB context with connection from your AppSettings 
            var optionsBuilder = new DbContextOptionsBuilder<FinDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new FinDbContext(optionsBuilder.Options);
        }
    }
}
