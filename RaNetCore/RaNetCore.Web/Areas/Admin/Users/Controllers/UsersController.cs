using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RaNetCore.Database.Interfaces;
using RaNetCore.Models.UserModels;
using RaNetCore.Models.UserModels.Enums;
using RaNetCore.Services.Interfaces;
using RaNetCore.Web.Areas.Admin.Users.ViewModels;
using RaNetCore.Web.BaseControllers;
using RaNetCore.Web.StartupConfig.Identity;

namespace RaNetCore.Web.Areas.Admin.Users.Controllers
{
    [Authorize(IdentityPolicies.SuperUserOnly)]
    [Route("api/[controller]")]
    public class UsersController : RaCrudController
        <ApplicationUser, UserDetailsViewModel>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UsersController(IRaNetCoreDbContext context,
                               UserManager<ApplicationUser> userManager,
                               IUserService modelService,
                               IMapper mapper)
            : base(context, modelService, mapper)
        {
            this.userManager = userManager;
            this.userService = modelService;
        }

        protected override async Task<UserDetailsViewModel> CreateAsync(UserDetailsViewModel model)
        {
            ApplicationUser entity = this.Mapper.Map<ApplicationUser>(model);

            ApplicationUser dbUser = await this.userService.Create(entity);

            await userManager.AddToRolesAsync(dbUser, model.Roles.Except(new[] { nameof(UserRoles.BasicUser) }));

            await userManager.UpdateAsync(dbUser);

            return this.Mapper.Map<UserDetailsViewModel>(dbUser);
        }

        protected override async Task<UserDetailsViewModel> UpdateAsync(UserDetailsViewModel model)
        {
            ApplicationUser applicationUser = await userManager
                   .FindByIdAsync(model.Id.ToString());

            List<string> oldRoles = new List<string>(
                    await userManager.GetRolesAsync(applicationUser));

            await userManager.RemoveFromRolesAsync(applicationUser, oldRoles.Except(model.Roles));
            await userManager.AddToRolesAsync(applicationUser, model.Roles.Except(oldRoles));

            applicationUser.FirstName = model.FirstName;
            applicationUser.LastName = model.LastName;
            applicationUser.Email = model.Email;

            await userManager.UpdateAsync(applicationUser);

            return this.Mapper.Map<UserDetailsViewModel>(applicationUser);
        }

        //public override async Task DeleteAsync(int id)
        //{
        //    //var entity = this.userService
        //    //    .GetById(id)
        //    //    .SingleOrDefaultAsync();

        //    await base.Delete(id);

        //    //return this.Mapper.Map<UserDetailsViewModel>(entity);
        //}

        protected override async Task<UserDetailsViewModel> AlterViewModelOnGet(UserDetailsViewModel viewModel)
        {
            await AttachUserRoles(viewModel);

            return viewModel;
        }

        private async Task AttachUserRoles(UserDetailsViewModel user)
        {
            ApplicationUser applicationUser = await userManager
                .FindByIdAsync(user.Id.ToString());

            List<string> roles = new List<string>(
                await userManager
                .GetRolesAsync(applicationUser));

            user.Roles = roles;
        }
    }
}
