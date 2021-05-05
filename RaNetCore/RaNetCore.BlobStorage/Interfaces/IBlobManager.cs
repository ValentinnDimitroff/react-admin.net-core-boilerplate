using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaNetCore.BlobStorage.Interfaces
{
    public interface IBlobManager
    {
        string UploadFileAndGetLink(string fileName, string folderName, string base64String);
        string UploadFileAndGetLink(string fileName, string folderName, Stream stream);
    }
}
