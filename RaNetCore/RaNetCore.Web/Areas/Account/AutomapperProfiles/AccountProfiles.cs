using AutoMapper;
using RaNetCore.Models.UserModels;
using RaNetCore.Web.Areas.Account.ViewModels;

namespace RaNetCore.Web.Areas.Account.AutomapperProfiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<ApplicationUser, AccountViewModel>();
        }
    }
}
