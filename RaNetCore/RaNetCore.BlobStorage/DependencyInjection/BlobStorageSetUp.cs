using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RaNetCore.BlobStorage.Interfaces;
using RaNetCore.BlobStorage.Providers.CloudinaryStorage;
using RaNetCore.BlobStorage.Providers.S3Storage;

namespace RaNetCore.BlobStorage.DependencyInjection
{
    public static class BlobStorageSetUp
    {
        public static IServiceCollection SetUpBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CloudinaryOptions>(
                options => configuration.GetSection(nameof(CloudinaryOptions)).Bind(options));

            services
                // Images Blob Storage
                .AddScoped<IImageBlobStorageProvider, CloudinaryBlobProvider>()
                .AddScoped<IImageBlobStorage, ImageBlobStorage>()
                // Files Blob Storage
                .AddScoped<IFileBlobStorageProvider, S3BlobProvider>()
                .AddScoped<IFileBlobStorage, FileBlobStorage>();

            return services;
        }
    }
}
