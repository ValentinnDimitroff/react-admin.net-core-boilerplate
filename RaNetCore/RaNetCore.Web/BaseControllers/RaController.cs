using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Database.Interfaces;
using RaNetCore.Services.BaseServices.Interfaces;
using RaNetCore.Web.BaseControllers.Helpers;
using RaNetCore.Web.BaseControllers.Interfaces;

namespace RaNetCore.Web.BaseControllers
{
    public class RaController<TBase, TDetails> : RaGetController<TBase, TDetails>, IRaController<TDetails>
        where TBase : class, IIdentifiable, new()
        where TDetails : class, IIdentifiable, new()
    {
        private string blobStorageFolderName;
        //private IBlobManager blobManager;

        public RaController(
            IRaNetCoreDbContext dbContext,
            IBaseModelService<TBase> modelService,
            IMapper mapper)
            : base(dbContext, modelService, mapper)
        {
        }

        //protected void SetUpBlobManager(IBlobManager blobManager, string folderName)
        //{
        //    this.blobManager = blobManager;
        //    this.blobStorageFolderName = folderName;
        //}        

        [HttpPost]
        public virtual async Task<TDetails> Post([FromBody] JObject model)
        {
            // TODO validate jobject
            try
            {
                TDetails convertedModel = JsonConvert.DeserializeObject<TDetails>(model.ToString());
                // Try upload images
                //if (this.blobManager != null) this.TryUploadImages(convertedModel);

                // Convert to Base Type
                TBase entity = this.Mapper.Map<TBase>(convertedModel);

                // Service.Create Db Record
                TBase newEntity = await this.ModelService.Create(entity).ConfigureAwait(true);

                return this.Mapper.Map<TDetails>(newEntity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<TDetails> Put([FromBody] JObject model)
        {
            if (model == null)
                throw new ArgumentException();

            try
            {
                TDetails convertedModel = JsonConvert.DeserializeObject<TDetails>(model.ToString());
                // Try upload images
                //if (this.blobManager != null) this.TryUploadImages(convertedModel);

                TBase entity = this.Mapper.Map<TBase>(convertedModel);
                TBase returnedEntity = await this.ModelService.Update(entity.Id, entity).ConfigureAwait(true);

                return this.Mapper.Map<TDetails>(returnedEntity);
            }
            catch (Exception ex)
            {
                //Non existing id
                throw;
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<bool> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException();

            try
            {
                await this.ModelService.Delete(id).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                throw;
            }

            return true;
        }

        //private void TryUploadImages(TDetails viewModel)
        //{
        //    RaHelpers.UploadImages(
        //          viewModel,
        //          this.blobManager.UploadFileAndGetLink,
        //          this.blobStorageFolderName);
        //}
    }
}
