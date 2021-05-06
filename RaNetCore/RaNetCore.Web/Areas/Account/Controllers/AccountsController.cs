using System.Threading.Tasks;
using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RaNetCore.BlobStorage.Interfaces;
using RaNetCore.Models.UserModels;
using RaNetCore.Services.Interfaces;
using RaNetCore.Web.Areas.Account.ViewModels;

namespace RaNetCore.Web.Areas.Account.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private const string ProfilePicturesBlobFolder = "profile-pictures";
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IImageBlobStorage imageBlobStorage;

        public AccountsController(
            IUserService userService,
            IMapper mapper,
            IImageBlobStorage imageBlobStorage)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.imageBlobStorage = imageBlobStorage;
        }

        [HttpGet("profile")]
        public async Task<AccountViewModel> GetProfile()
        {
            return this.mapper.Map<AccountViewModel>(
                await this.GetDbUser()
            );
        }

        [HttpPost("profile")]
        public async Task<AccountViewModel> EditProfile([FromBody]JObject model)
        {
            AccountViewModel formModel = JsonConvert.DeserializeObject<AccountViewModel>(model.ToString());

            ApplicationUser dbUser = await this.GetDbUser();

            dbUser.FirstName = formModel.FirstName;
            dbUser.LastName = formModel.LastName;
            dbUser.Email = formModel.Email;
            dbUser.PhoneNumber = formModel.PhoneNumber;

            return await this.UpdateDbUserAndReturn(dbUser);
        }

        [HttpPost("[action]")]
        public async Task<AccountViewModel> UploadPicture([FromBody]JObject model)
        {
            ApplicationUser dbUser = await this.GetDbUser();

            string base64Image = model?.Value<string>("base64Image");
            int commaIndex = base64Image.IndexOf(",");
            string base64Only = base64Image.Remove(0, commaIndex + 1);

            dbUser.Picture = this.imageBlobStorage
                .UploadFileAndGetLink(dbUser.Id.ToString(), ProfilePicturesBlobFolder, base64Only);

            return await this.UpdateDbUserAndReturn(dbUser);
        }

        // TODO: Change Password

        // Private Methods

        private async Task<ApplicationUser> GetDbUser()
            => await this.userService
                          .GetCurrentUser()
                          .SingleOrDefaultAsync();

        private async Task<AccountViewModel> UpdateDbUserAndReturn(ApplicationUser user)
            => this.mapper.Map<AccountViewModel>(
                await this.userService
                          .Update(user.Id, user)
            );

    }
}
