using Microsoft.AspNetCore.Identity;

namespace RaNetCore.Models.UserModels
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
            : base()
        {

        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {

        }
    }
}
