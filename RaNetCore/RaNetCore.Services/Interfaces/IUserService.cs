using System.Linq;

using RaNetCore.Models.UserModels;
using RaNetCore.Services.BaseServices.Interfaces;

namespace RaNetCore.Services.Interfaces
{
    public interface IUserService : IBaseModelService<ApplicationUser>
    {
        IQueryable<ApplicationUser> GetCurrentUser();
    }
}
