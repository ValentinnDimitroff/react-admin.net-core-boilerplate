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

            //    services.AddOptions<CookieAuthenticationOptions>(
            //                CookieAuthenticationDefaults.AuthenticationScheme)
            //.Configure<IMyService>((options, myService) =>
            //{
            //    options.LoginPath = myService.GetLoginPath();
            //});

            //services.Configure<CloudinaryOptions>(configuration?.GetSection(nameof(CloudinaryOptions)));

            services.AddOptions<CloudinaryOptions>(nameof(CloudinaryOptions));

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
