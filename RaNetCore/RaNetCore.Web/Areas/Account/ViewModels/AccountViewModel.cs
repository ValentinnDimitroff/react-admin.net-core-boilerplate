using RaNetCore.Common.Entities;

namespace RaNetCore.Web.Areas.Account.ViewModels
{
    public class AccountViewModel : IdentifiableModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }
    }
}
