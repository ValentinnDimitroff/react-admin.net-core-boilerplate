using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RaNetCore.Web.DbInit.Extensions
{
    public static class DbInitExtensions
    {
        public static TDbContext EnsureDbCreatedAndMigrated<TDbContext>(this IServiceScope serviceScope)
            where TDbContext : DbContext
        {
            TDbContext dbContext = serviceScope
                        .ServiceProvider
                        .GetService<TDbContext>();

            dbContext
                .Database
                .Migrate(); // ensures created and applies migrations

            return dbContext;
        }
    }
}
