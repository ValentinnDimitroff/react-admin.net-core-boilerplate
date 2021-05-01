using AutoMapper;

using RaNetCore.Models.UserModels;
using RaNetCore.Web.Areas.Admin.Users.ViewModels;

namespace RaNetCore.Web.Areas.Admin.Users.AutomapperProfiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<ApplicationUser, UserDetailsViewModel>()
                .ReverseMap();
        }
    }
}
