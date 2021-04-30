using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RaNetCore.Database;
using RaNetCore.Database.SqlScripts;
using RaNetCore.Models.UserModels;
using RaNetCore.Models.UserModels.Enums;

namespace RaNetCore.Web.DbInit
{
    public static class DbInitilizer
    {
        public static async Task Initialize(RaNetCoreDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            // Seed roles and users 
            UserManager<ApplicationUser> userManager = serviceProvider
                .GetService<UserManager<ApplicationUser>>();
            RoleManager<ApplicationRole> roleManager = serviceProvider
                .GetService<RoleManager<ApplicationRole>>();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);

            // Change Tables Collation - fixes Ef bug with some MySql versions
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));

            //await dbContext
            //    .Database
            //    .ExecuteSqlCommandAsync($"{OnCreationSqlScripts.ChangeCollation(builder.InitialCatalog)}");
        }

        private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (string roleName in Enum.GetNames(typeof(UserRoles)))
            {
                if (!(await roleManager.RoleExistsAsync(roleName)))
                {
                    ApplicationRole newRole = new ApplicationRole(roleName);

                    IdentityResult roleResult = await roleManager
                        .CreateAsync(newRole);
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            foreach ((string Password, ApplicationUser User, List<UserRoles> Roles) in SeedDataSets.Users)
            {
                if (userManager.FindByNameAsync(User.UserName).Result == null)
                {
                    IdentityResult result = await userManager
                        .CreateAsync(User, Password);

                    if (result.Succeeded)
                    {
                        await userManager
                            .AddToRolesAsync(User, Roles.Select(x => x.ToString()));
                    }
                }
            }
        }
    }
}
