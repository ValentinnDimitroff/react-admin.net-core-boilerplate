using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using RaNetCore.Database;
using RaNetCore.Models.UserModels;
using RaNetCore.Models.UserModels.Enums;
using RaNetCore.Web.StartupConfig.Identity.AppSettingsModels;

namespace RaNetCore.Web.StartupConfig.Identity
{
    public static class IdentitySetUp
    {
        public static IServiceCollection SetUpIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<RaNetCoreDbContext>()
                .AddDefaultTokenProviders();

            // Configure identity options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            // Load jwt settings from appsettings 
            IConfigurationSection jwtSettingsSection = configuration
                .GetSection("JwtSettings");

            services.Configure<JwtSettings>(jwtSettingsSection);

            // Add Jwt Authentication
            JwtSettings jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            byte[] jwtKeyByteArr = Encoding.UTF8.GetBytes(jwtSettings.JwtKey);

            JwtSecurityTokenHandler
                .DefaultInboundClaimTypeMap
                .Clear(); // => remove default claims

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.JwtIssuer,
                        ValidAudience = jwtSettings.JwtIssuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtKeyByteArr),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                })
                .AddCookie();

            // Apply policies
            services.AddAuthorization(options =>
                {
                    options.AddPolicy(
                        IdentityPolicies.AdminOnly,
                        policy => policy.RequireRole(nameof(UserRoles.Admin)));
                    options.AddPolicy(
                        IdentityPolicies.SuperUserOnly,
                        policy => policy.RequireRole(nameof(UserRoles.Admin), nameof(UserRoles.SuperUser)));
                    options.AddPolicy(
                        IdentityPolicies.Standard,
                        policy => policy.RequireRole(
                            nameof(UserRoles.Admin),
                            nameof(UserRoles.SuperUser),
                            nameof(UserRoles.BasicUser)
                            ));
                });

            return services;
        }
    }
}
