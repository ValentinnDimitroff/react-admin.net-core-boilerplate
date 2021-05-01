using System.ComponentModel.DataAnnotations;

namespace RaNetCore.Web.Areas.Authentication.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
