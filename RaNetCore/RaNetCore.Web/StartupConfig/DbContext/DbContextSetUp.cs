
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using RaNetCore.Database;
using RaNetCore.Database.Interfaces;

namespace RaNetCore.Web.StartupConfig.DbContext
{
    public static class DbContextSetUp
    {
        public static IServiceCollection SetUpDbContext(this IServiceCollection services, string connectionString)
        {
            // HttpContextAccessor is used in DbContext to add Timestamps
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add DbContext
            services.AddEntityFrameworkMySql()
                .AddDbContext<RaNetCoreDbContext>(
                    options => options.UseMySql(connectionString));

            // Register DbContext by interface
            services.AddScoped<IRaNetCoreDbContext>(
                provider => provider.GetService<RaNetCoreDbContext>());

            return services;
        }
    }
}
