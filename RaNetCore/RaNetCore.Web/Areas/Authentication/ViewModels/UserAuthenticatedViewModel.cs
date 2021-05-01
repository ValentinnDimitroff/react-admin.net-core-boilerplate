using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaNetCore.Web.Areas.Authentication.ViewModels
{
    public class UserAuthenticatedViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        public string Token { get; set; }
    }
}
