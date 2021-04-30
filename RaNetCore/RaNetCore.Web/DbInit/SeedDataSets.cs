using System.Collections.Generic;

using RaNetCore.Models.UserModels;
using RaNetCore.Models.UserModels.Enums;

namespace RaNetCore.Web.DbInit
{
    public class SeedDataSets
    {
        public static List<(string Password, ApplicationUser User, List<UserRoles> Roles)> Users =>
                new List<(string Password, ApplicationUser User, List<UserRoles> Roles)>()
            {
                (
                    "MySecretPass1!",
                    new ApplicationUser("admin@admin.com")
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                    },
                    new List<UserRoles>() { UserRoles.Admin }
                )
            };
    }
}
