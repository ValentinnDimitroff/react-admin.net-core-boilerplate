using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Database.Interfaces;
using RaNetCore.Services.BaseServices.Interfaces;
using RaNetCore.Web.BaseControllers.Extensions;
using RaNetCore.Web.BaseControllers.Interfaces;

namespace RaNetCore.Web.BaseControllers
{
    public class RaGetController<TBase, TDetails> : ControllerBase, IRaGetController<TDetails>
        where TBase : class, IIdentifiable, new()
        where TDetails : class, IIdentifiable, new()
    {
        public RaGetController(
             IRaNetCoreDbContext dbContext,
             IBaseModelService<TBase> modelService,
             IMapper mapper)
        {
            this.DbContext = dbContext;
            this.ModelService = modelService;
            this.Mapper = mapper;
        }

        // Properties
        protected IRaNetCoreDbContext DbContext { get; private set; }
        protected IMapper Mapper { get; private set; }
        protected IBaseModelService<TBase> ModelService { get; private set; }
        // Place to plug in custom sorting implementation
        protected Func<string, string, IQueryable<TBase>, IQueryable<TBase>> CustomSorting { get; set; }

        // Actions
        [HttpGet]
        public async Task<IEnumerable<TDetails>> Get(string filter = "", string range = "", string sort = "")
        {
            return await this.GetAll(filter, range, sort);
        }

        [HttpGet("{id}")]
        public async Task<TDetails> Get(int id)
        {
            return await this.GetById(id);
        }

        // Methods - overrideable
        protected virtual async Task<IEnumerable<TDetails>> GetAll(string filter = "", string range = "", string sort = "")
        {
            // Retrieve and process db entites
            List<TBase> fetchedRows = await this.ModelService
                                                .GetAll(filter)
                                                .RaSort(sort, this.CustomSorting)
                                                .RaPaginate(range, Response)
                                                .ToListAsync();

            // TODO - insert before fetching
            // Convert db entites to view models
            IEnumerable<TDetails> result = await Task.WhenAll(
                this.FilterFetchedQuery(fetchedRows)
                    .Select(x => this.Mapper.Map<TDetails>(x))
                    .Select(async x => await this.AlterViewModelOnGet(x))
                );

            return result;
        }

        protected virtual async Task<TDetails> GetById(int id)
        {
            try
            {
                // Retrieve entity from db
                TDetails entity = await this.ModelService
                    .GetById(id)
                    .Select(x => this.Mapper.Map<TDetails>(x))
                    .SingleOrDefaultAsync();

                // TODO handle better
                if (entity == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    throw new Exception();
                }

                return await this.AlterViewModelOnGet(entity);
            }
            // TODO handle better
            catch (Exception ex)
            {
                var a = ex.Message;
                throw;
            }
        }

        protected virtual async Task<TDetails> AlterViewModelOnGet(TDetails viewModel) => viewModel;

        protected virtual IEnumerable<TBase> FilterFetchedQuery(List<TBase> result) => result;

    }
}
