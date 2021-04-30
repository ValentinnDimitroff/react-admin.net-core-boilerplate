using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Models.UserModels;

namespace RaNetCore.Database.Abstractions
{
    /// <summary>
    /// DbContext adding timestamps on create and update for each object inheriting the ITimestampsBaseEntity
    /// </summary>
    public abstract class TimestampsDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TimestampsDbContext(DbContextOptions<RaNetCoreDbContext> options,
           IHttpContextAccessor httpContextAccessor)
           : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges()
        {
            this.AddTimestamps<ITimestampsBaseEntity>();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.AddTimestamps<ITimestampsBaseEntity>();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps<T>() where T : ITimestampsBaseEntity
        {
            // Get all new or updated entities which support timestamping
            IEnumerable<EntityEntry> entities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is T
                            && (x.State == EntityState.Added || x.State == EntityState.Modified));

            // Get current operation actuator's id
            string userStrId = this.httpContextAccessor
                ?.HttpContext
                ?.User
                ?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

            // If id is present in the httpContext parse to int
            int.TryParse(userStrId, out int userId);

            foreach (var entity in entities)
            {
                // Timestamps for new entites
                if (entity.State == EntityState.Added)
                {
                    ((T)entity.Entity).CreatedDate = DateTime.UtcNow;
                    ((T)entity.Entity).CreatedBy = userId;
                }

                // Timestamps for updated entites
                ((T)entity.Entity).ModifiedDate = DateTime.UtcNow;
                ((T)entity.Entity).ModifiedBy = userId;
            }
        }
    }
}
