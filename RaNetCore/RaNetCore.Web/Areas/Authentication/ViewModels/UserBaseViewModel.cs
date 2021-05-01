using System.ComponentModel.DataAnnotations;

namespace RaNetCore.Web.Areas.Authentication.ViewModels
{
    public abstract class UserBaseViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
