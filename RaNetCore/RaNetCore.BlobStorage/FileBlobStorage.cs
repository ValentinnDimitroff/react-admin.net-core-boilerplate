using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RaNetCore.BlobStorage.Abstraction;
using RaNetCore.BlobStorage.Interfaces;

namespace RaNetCore.BlobStorage
{
    public class FileBlobStorage : BlobManager
    {
        public FileBlobStorage(IFileBlobStorageProvider provider) : base(provider)
        {
        }
    }
}
