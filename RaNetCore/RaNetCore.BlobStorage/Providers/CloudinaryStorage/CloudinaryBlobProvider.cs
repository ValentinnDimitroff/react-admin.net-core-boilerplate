using System;
using System.IO;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using RaNetCore.BlobStorage.Interfaces;

using Microsoft.Extensions.Options;

namespace RaNetCore.BlobStorage.Providers.CloudinaryStorage
{
    public class CloudinaryBlobProvider : IImageBlobStorageProvider
    {
        private static readonly string MissingOptionsErrorMsg = $"Options are mandatory for initlizing {nameof(CloudinaryBlobProvider)}";

        private readonly Cloudinary cloudinary;

        public CloudinaryBlobProvider(IOptions<CloudinaryOptions> options)
        {
            CloudinaryOptions _options = options?.Value ?? throw new ArgumentNullException(MissingOptionsErrorMsg);

            Account account = new Account(_options.CloudName, _options.ApiKey, _options.ApiSecret);

            this.cloudinary = new Cloudinary(account);
        }

        public string UploadFile(string fileName, string folderName, Stream stream)
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Folder = folderName,
                UniqueFilename = true,
            };

            ImageUploadResult result = this.cloudinary.Upload(uploadParams);

            return result
                ?.SecureUrl
                ?.AbsoluteUri ?? throw new ArgumentNullException("Picture upload failed - url is not returned");
        }
    }
}
