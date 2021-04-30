using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Database.Extensions;
using RaNetCore.Database.Interfaces;
using RaNetCore.Services.BaseServices.Helpers;
using RaNetCore.Services.BaseServices.Interfaces;

namespace RaNetCore.Services.BaseServices
{
    public class BaseModelService<TEntity> : IBaseModelService<TEntity>
        where TEntity : class, IIdentifiable
    {
        private bool creatorOnlyAccess;
        private readonly FilterQueryBuilder<TEntity> filterQueryBuilder;

        public BaseModelService(IRaNetCoreDbContext DbContext, IHttpContextService httpContextService)
        {
            this.DbContext = DbContext;
            this.HttpContextService = httpContextService;

            this.filterQueryBuilder = new FilterQueryBuilder<TEntity>(
                this.DbContext
                .Set<TEntity>()
                .AsNoTracking()
            );
        }

        // Properties

        protected IRaNetCoreDbContext DbContext { get; private set; }
        protected IHttpContextService HttpContextService { get; private set; }

        // CRUD Methods

        public virtual IQueryable<TEntity> GetAll(string filter = "")
        {
            // Filtering
            if (!string.IsNullOrEmpty(filter))
            {
                JObject filterJsonDict = (JObject)JsonConvert.DeserializeObject(filter);

                filterQueryBuilder.PerformGenericSearch(filterJsonDict);

                foreach (KeyValuePair<string, JToken> filterRow in filterJsonDict)
                {
                    (string propName, JToken propValue) = FilterHelpers.ParseFilterOption(filterRow);

                    // Detect if custom filter key is passed
                    if (propName.StartsWith("_"))
                    {
                        filterQueryBuilder.AttachQueryToBuilder(
                            this.GetCustomFilterFunc(propName, propValue));

                        continue;
                    }

                    filterQueryBuilder.HandleDefault(propName, propValue);
                }
            }

            // TODO - remove (nonsense?)
            IQueryable<TEntity> finalQuery = filterQueryBuilder
                .Query?
                .AsNoTracking()
                ?? this.DbContext
                       .Set<TEntity>()
                       .AsNoTracking();

            // TODO - add description - auto includes?
            finalQuery
                .Include(this.DbContext
                             .GetIncludePaths(typeof(TEntity)));

            if (this.creatorOnlyAccess)
            {
                return this.RestrictAccessFilter(
                            this.IncludeEntites(finalQuery));
            }
            else
            {
                return this.IncludeEntites(finalQuery);
            }
        }

        public virtual IQueryable<TEntity> GetById(int id)
        {
            IQueryable<TEntity> retrievedEntity = this.DbContext
                .Set<TEntity>()
                .AsNoTracking()
                .Where(e => e.Id == id);

            if (this.creatorOnlyAccess)
            {
                return this.RestrictAccessFilter(
                            this.IncludeEntites(retrievedEntity));
            }
            else
            {
                return this.IncludeEntites(retrievedEntity);
            }
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            await this.DbContext
                .Set<TEntity>()
                .AddAsync(entity);

            await this.DbContext
                .SaveChangesAsync();

            return await this.GetById(entity.Id)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<TEntity> Update(int id, TEntity entity, bool creatorOnlyAllowed = false)
        {
            try
            {
                if (creatorOnlyAllowed || this.creatorOnlyAccess)
                    this.CheckIfCurrentUserIsCreator(entity);

                this.DbContext
                    .Set<TEntity>()
                    .Update(entity);

                await this.DbContext
                    .SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
                {
                    throw new ArgumentException($"{typeof(TEntity).Name} entity for the given id is missing. Cannot perform an update!");
                }
                else
                {
                    throw;
                }
            }

            return await this.GetById(entity.Id)
                .SingleOrDefaultAsync();
        }

        public virtual async Task Delete(int id, bool creatorOnlyAllowed = false)
        {
            TEntity entity = await GetById(id)
                .SingleOrDefaultAsync();

            if (creatorOnlyAllowed || this.creatorOnlyAccess)
                this.CheckIfCurrentUserIsCreator(entity);

            if (this.IsSoftDeletable(entity))
            {
                ISoftDeletable softDeletableEntity = (ISoftDeletable)entity;
                softDeletableEntity.IsDeleted = true;

                await this.Update(entity.Id, (TEntity)softDeletableEntity);

                return;
            }

            this.DbContext
                .Set<TEntity>()
                .Remove(entity);

            await this.DbContext
                .SaveChangesAsync();
        }

        // Protected Methods - overrideable

        protected virtual Func<TEntity, bool> GetCustomFilterFunc(string propName, JToken propValue) => null;
        protected virtual IQueryable<TEntity> IncludeEntites(IQueryable<TEntity> query) => query;

        // Protected Methods

        protected void UpdateInnerCollection<TCrossEntity>(Func<TCrossEntity, bool> compareFnc,
            IEnumerable<TCrossEntity> updatedCollection)
            where TCrossEntity : class
        {
            IEnumerable<TCrossEntity> deletedItems = this.DbContext
                .Set<TCrossEntity>()
                .Where(compareFnc)
                .Except(updatedCollection);

            this.DbContext
                .Set<TCrossEntity>()
                .RemoveRange(deletedItems);
        }
        
        protected void UpdateManyToMany<TCrossEntity>(TEntity updatedEntity, Func<TCrossEntity, int> compareFnc)
            where TCrossEntity : class
        {
            INavigation crossEntityNavigation = this.DbContext
               .Model
               .FindEntityType(typeof(TEntity))
               .GetNavigations()
               .SingleOrDefault(x => x.PropertyInfo
                                      .PropertyType
                                      .GenericTypeArguments
                                      .Any(arg => arg == typeof(TCrossEntity)));

            TEntity originalEntity = this.DbContext
                .Set<TEntity>()
                .Where(x => x.Id == updatedEntity.Id)
                .AsNoTracking()
                .Include(crossEntityNavigation.Name)
                .AsNoTracking()
                .SingleOrDefault();

            this.DbContext.TryUpdateManyToMany(
                // Original collection values
                (IEnumerable<TCrossEntity>)crossEntityNavigation
                    .PropertyInfo
                    .GetValue(originalEntity),
                // New collection values
                (IEnumerable<TCrossEntity>)crossEntityNavigation
                    .PropertyInfo
                    .GetValue(updatedEntity),
                // delegate to decide which are the common items
                compareFnc);
        }        

        // Private Methods

        private bool EntityExists(int id)
        {
            return this.DbContext
                .Set<TEntity>()
                .Any(e => (int)typeof(TEntity).GetProperty("Id").GetValue(e) == id);
        }

        private bool IsSoftDeletable(TEntity entity)
        {
            return typeof(TEntity).GetInterface(nameof(ISoftDeletable)) != null;
        }

        #region RestrictAccess Methods

        protected Func<TEntity, bool> RestrictAccessPredicate { get; private set; }

        protected int GetCurrentUserId() => this.HttpContextService.User.UserId;

        protected void SetCreatorAccessOnly() => this.SetCreatorAccessOnly(x => ((ITimestampsBaseEntity)x).CreatedBy == this.GetCurrentUserId());

        protected void SetCreatorAccessOnly(Func<TEntity, bool> restrictAccessPredicate)
        {
            this.RestrictAccessPredicate = restrictAccessPredicate;
            this.creatorOnlyAccess = true;
        }

        protected void CheckIfCurrentUserIsCreator(TEntity entity)
        {
            if (typeof(TEntity).GetInterface(nameof(ITimestampsBaseEntity)) != null)
            {
                ITimestampsBaseEntity convertedEntity = (ITimestampsBaseEntity)entity;
                if (this.GetCurrentUserId() != convertedEntity.CreatedBy)
                {
                    throw new UnauthorizedAccessException("Only the creator of the record can alter it!");
                }
            }
        }

        private IQueryable<TEntity> RestrictAccessFilter(IQueryable<TEntity> query)
        {
            if (typeof(TEntity).GetInterface(nameof(ITimestampsBaseEntity)) is null)
                return query;

            return query
                .Where(x => this.RestrictAccessPredicate
                                .Invoke(x));
        }

        #endregion

    }
}
