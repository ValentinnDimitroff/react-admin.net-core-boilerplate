using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using RaNetCore.Common.Entities.Interfaces;

namespace RaNetCore.Models.UserModels
{
    public class ApplicationUser : IdentityUser<int>, IIdentifiable
    {
        public ApplicationUser() : base()
        {
        }

        public ApplicationUser(string email) : base()
        {
            this.UserName = email;
            this.Email = email;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Picture { get; set; }

        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";

    }
}
