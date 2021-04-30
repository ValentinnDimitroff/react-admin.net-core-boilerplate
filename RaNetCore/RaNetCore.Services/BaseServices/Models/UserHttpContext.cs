using System.Collections.Generic;

using RaNetCore.Models.UserModels.Enums;

namespace RaNetCore.Services.BaseServices.Models
{
    public class UserHttpContext
    {
        public int UserId { get; set; }

        public string UserIdStr { get; set; }

        public string Username { get; set; }

        public IEnumerable<UserRoles> Roles { get; set; }
    }
}
