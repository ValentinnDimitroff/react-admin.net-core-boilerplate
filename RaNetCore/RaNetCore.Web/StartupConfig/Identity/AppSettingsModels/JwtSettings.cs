namespace RaNetCore.Web.StartupConfig.Identity.AppSettingsModels
{
    public class JwtSettings
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtExpireDays { get; set; }
    }
}
