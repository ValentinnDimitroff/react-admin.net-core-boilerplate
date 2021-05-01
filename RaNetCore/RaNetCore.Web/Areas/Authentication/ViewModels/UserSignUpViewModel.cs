using System.ComponentModel.DataAnnotations;

namespace Dve.Web.Areas.Authentication.ViewModels
{
    public class UserSignUpViewModel : UserBaseViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Confirmed password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
