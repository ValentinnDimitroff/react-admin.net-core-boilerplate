using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using RaNetCore.BlobStorage.Interfaces;
using RaNetCore.Common.Entities.Interfaces;
using RaNetCore.Database.Interfaces;
using RaNetCore.Services.BaseServices.Interfaces;
using RaNetCore.Web.BaseControllers.Helpers;
using RaNetCore.Web.BaseControllers.Interfaces;

namespace RaNetCore.Web.BaseControllers
{
    public class RaBlobCrudController<TBase, TDetails> : RaCrudController<TBase, TDetails>, IRaController<TDetails>
        where TBase : class, IIdentifiable, new()
        where TDetails : class, IIdentifiable, new()
    {
        private string imageBlobStorageFolderName;
        private IImageBlobStorage imageBlobStorage;
        private string fileBlobStorageFolderName;
        private IFileBlobStorage fileBlobStorage;

        public RaBlobCrudController(
            IRaNetCoreDbContext dbContext,
            IBaseModelService<TBase> modelService,
            IMapper mapper)
            : base(dbContext, modelService, mapper)
        {
        }

        // SetUp Methods - to be called in child's constructor

        protected void SetUpImageBlobStorage(IImageBlobStorage blobStorage, string folderName)
        {
            this.imageBlobStorage = blobStorage;
            this.imageBlobStorageFolderName = folderName;
        }

        protected void SetUpFileBlobStorage(IFileBlobStorage blobStorage, string folderName)
        {
            this.fileBlobStorage = blobStorage;
            this.fileBlobStorageFolderName = folderName;
        }

        // Extending RaCrud Methods

        protected override async Task<TDetails> CreateAsync(TDetails model)
        {
            
            if (this.imageBlobStorage != null) 
                this.TryUploadImages(model);

            return await base.CreateAsync(model);
        }

        protected override async Task<TDetails> UpdateAsync(TDetails model)
        {
            if (this.imageBlobStorage != null)
                this.TryUploadImages(model);

            return await base.UpdateAsync(model);
        }

        // Private Methods

        private void TryUploadImages(TDetails viewModel)
        {
            RaHelpers.UploadImages(
                  viewModel,
                  this.imageBlobStorage.UploadFileAndGetLink,
                  this.imageBlobStorageFolderName);
        }
    }
}
