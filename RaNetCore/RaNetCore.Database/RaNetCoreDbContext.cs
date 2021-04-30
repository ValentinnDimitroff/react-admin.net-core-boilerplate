using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using RaNetCore.Database.Abstractions;
using RaNetCore.Database.Interfaces;

namespace RaNetCore.Database
{
    public class RaNetCoreDbContext : TimestampsDbContext, IRaNetCoreDbContext
    {
        public RaNetCoreDbContext(DbContextOptions<RaNetCoreDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options, httpContextAccessor)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
