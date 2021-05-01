using System.Collections.Generic;

using RaNetCore.Common.Entities;

namespace RaNetCore.Web.Areas.Admin.Users.ViewModels
{
    public class UserDetailsViewModel : IdentifiableModel
    {
        public string FullName => $"{this.FirstName} {this.LastName}";

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

    }
}
