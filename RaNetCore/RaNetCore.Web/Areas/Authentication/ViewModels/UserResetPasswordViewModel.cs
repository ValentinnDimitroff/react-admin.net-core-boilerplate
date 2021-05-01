using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaNetCore.Web.Areas.Authentication.ViewModels
{    
    public class UserResetPasswordViewModel : UserBaseViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Confirmed password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
