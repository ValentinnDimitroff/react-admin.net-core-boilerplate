using Microsoft.Extensions.DependencyInjection;

namespace RaNetCore.Web.StartupConfig.ThirdParties
{
    public static class ThirdPartiesSetUp
    {
        public static IServiceCollection SetUpThirdParties(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { cfg.AllowNullCollections = true; }, typeof(Startup));

            return services;
        }
    }
}
