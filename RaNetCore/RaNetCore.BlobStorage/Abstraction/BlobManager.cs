using System;
using System.IO;

using RaNetCore.BlobStorage.Interfaces;

namespace RaNetCore.BlobStorage.Abstraction
{
    public abstract class BlobManager : IBlobManager, IFileBlobStorage, IImageBlobStorage
    {
        private readonly IBlobStorageProvider provider;

        public BlobManager(IBlobStorageProvider provider)
        {
            this.provider = provider;
        }

        public string UploadFileAndGetLink(string fileName, string folderName, string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return this.UploadFileAndGetLink(fileName, folderName, stream);
            }
        }

        public string UploadFileAndGetLink(string fileName, string folderName, Stream stream)
            => this.provider.UploadFile(fileName, folderName, stream);

    }
}
