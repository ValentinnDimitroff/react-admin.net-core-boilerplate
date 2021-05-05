using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaNetCore.BlobStorage.Interfaces;

namespace RaNetCore.BlobStorage.Providers.S3Storage
{
    public class S3BlobProvider : IFileBlobStorageProvider
    {
        public string UploadFile(string fileName, string folderName, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
