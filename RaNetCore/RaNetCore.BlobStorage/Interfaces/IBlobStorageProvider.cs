using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaNetCore.BlobStorage.Interfaces
{
    public interface IBlobStorageProvider
    {
        string UploadFile(string fileName, string folderName, Stream stream);
    }
}
