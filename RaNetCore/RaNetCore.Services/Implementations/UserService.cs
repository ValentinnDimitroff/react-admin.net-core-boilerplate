using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using RaNetCore.Database.Interfaces;
using RaNetCore.Models.UserModels;
using RaNetCore.Models.UserModels.Enums;
using RaNetCore.Services.BaseServices;
using RaNetCore.Services.BaseServices.Interfaces;
using RaNetCore.Services.Interfaces;

namespace RaNetCore.Services.Implementations
{
    public class UserService : BaseModelService<ApplicationUser>, IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IRaNetCoreDbContext dbContext,
            IHttpContextService httpContextService,
            UserManager<ApplicationUser> userManager)
            : base(dbContext, httpContextService)
        {
            this.userManager = userManager;
        }

        public override async Task<ApplicationUser> Create(ApplicationUser user)
        {
            IdentityResult result = await this.userManager.CreateAsync(user, "Def@ultP@sS~007");

            if (result.Succeeded)
            {
                // Assing the most basic role to the newly created User
                await this.userManager.AddToRoleAsync(user, UserRoles.BasicUser.ToString());
            }
            else
            {
                throw new Exception(string.Join(", ", result.Errors.Select(x => x.Description)));
            }

            return user;
        }

        public IQueryable<ApplicationUser> GetCurrentUser()
            => this.GetById(this.GetCurrentUserId());
    }
}
