using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using RaNetCore.Database;
using RaNetCore.Web.DbInit;
using RaNetCore.Web.DbInit.Extensions;

namespace RaNetCore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Build host
            IHost host = CreateHostBuilder(args).Build();

            // Initilize Db
            using (IServiceScope serviceScope = host.Services.CreateScope())
            {
                try
                {
                    RaNetCoreDbContext dbContext = serviceScope
                        .EnsureDbCreatedAndMigrated<RaNetCoreDbContext>();

                    DbInitilizer
                        .Initialize(dbContext, serviceScope.ServiceProvider)
                        .Wait();
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing Database!");
                }
            }

            // Run host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
