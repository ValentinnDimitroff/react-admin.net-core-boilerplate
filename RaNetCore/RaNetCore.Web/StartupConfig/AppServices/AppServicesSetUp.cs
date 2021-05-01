using Microsoft.Extensions.DependencyInjection;

using RaNetCore.Services.BaseServices;
using RaNetCore.Services.BaseServices.Interfaces;
using RaNetCore.Services.Implementations;
using RaNetCore.Services.Interfaces;

namespace RaNetCore.Web.StartupConfig.AppServices
{
    public static class AppServicesSetUp
    {
        public static void SetUpAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseModelService<>), typeof(BaseModelService<>));
            services.AddScoped<IHttpContextService, HttpContextService>();

            services.AddScoped<IUserService, UserService>();
        }
    }
}
