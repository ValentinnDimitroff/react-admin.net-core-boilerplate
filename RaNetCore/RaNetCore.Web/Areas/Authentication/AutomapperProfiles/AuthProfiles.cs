using AutoMapper;
using RaNetCore.Models.UserModels;
using RaNetCore.Web.Areas.Authentication.ViewModels;

namespace RaNetCore.Web.Areas.Authentication.AutomapperProfiles
{
    public class AuthProfiles : Profile
    {
        public AuthProfiles()
        {
            CreateMap<ApplicationUser, UserAuthenticatedViewModel>();
        }   
    }
}
