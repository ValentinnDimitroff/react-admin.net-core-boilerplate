using System;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Database.Interfaces;
using RaNetCore.Services.BaseServices.Interfaces;
using RaNetCore.Web.BaseControllers.Interfaces;

namespace RaNetCore.Web.BaseControllers
{
    public abstract class RaCrudController<TBase, TDetails> : RaGetController<TBase, TDetails>, IRaController<TDetails>
        where TBase : class, IIdentifiable, new()
        where TDetails : class, IIdentifiable, new()
    {
        public RaCrudController(
            IRaNetCoreDbContext dbContext,
            IBaseModelService<TBase> modelService,
            IMapper mapper)
            : base(dbContext, modelService, mapper)
        {
        } 

        // Actions

        [HttpPost]
        public async Task<TDetails> Post([FromBody] JObject model)
        {
            // TODO validate jobject
            if (model is null)
                throw new ArgumentException();

            try
            {
                TDetails convertedModel = JsonConvert.DeserializeObject<TDetails>(model.ToString());

                return await CreateAsync(convertedModel);
            }
            catch (Exception ex)
            {
                // TODO handle exception
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<TDetails> Put([FromBody] JObject model)
        {
            if (model == null)
                throw new ArgumentException();

            try
            {
                TDetails convertedModel = JsonConvert.DeserializeObject<TDetails>(model.ToString());

                return await this.UpdateAsync(convertedModel);
            }
            catch (Exception ex)
            {
                //Non existing id
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException();

            try
            {
                await this.DeleteAsync(id);

                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }
        }


        // Methods - overrideable

        protected virtual async Task<TDetails> CreateAsync(TDetails model)
        {
            // Convert to Base Type
            TBase entity = this.Mapper.Map<TBase>(model);

            // Service.Create Db Record
            TBase newEntity = await this.ModelService.Create(entity).ConfigureAwait(true);

            return this.Mapper.Map<TDetails>(newEntity);
        }

        protected virtual async Task<TDetails> UpdateAsync(TDetails model)
        {
            TBase entity = this.Mapper.Map<TBase>(model);

            TBase returnedEntity = await this.ModelService.Update(entity.Id, entity);

            return this.Mapper.Map<TDetails>(returnedEntity);
        }

        protected virtual async Task DeleteAsync(int id)
        {
            await this.ModelService.Delete(id);
        }

    }
}
