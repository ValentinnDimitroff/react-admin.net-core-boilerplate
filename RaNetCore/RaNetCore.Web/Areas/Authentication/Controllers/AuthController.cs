using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using RaNetCore.Models.UserModels;
using RaNetCore.Models.UserModels.Enums;
using RaNetCore.Web.Areas.Authentication.ViewModels;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Web;
using RaNetCore.Web.StartupConfig.Identity.AppSettingsModels;

namespace RaNetCore.Web.Areas.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtSettings jwtSettings;
        private readonly IMapper mapper;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtSettings = jwtSettings?.Value;
            this.mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<object> Login([FromBody] UserLoginViewModel model)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await signInManager
                .PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                ApplicationUser retrievedUser = userManager.Users
                    .SingleOrDefault(r => r.Email == model.Email);

                return await this.ReturnAuthenticatedUserAsync(retrievedUser);
            }

            return Unauthorized(); //new InvalidSignInCredentials()
        }

        [HttpPost("[action]")]
        public async Task<object> SignUp([FromBody] UserSignUpViewModel model)
        {
            ApplicationUser user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
            };

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assing the most basic role to the newly created User
                await this.userManager.AddToRoleAsync(user, UserRoles.BasicUser.ToString());

                return await this.ReturnAuthenticatedUserAsync(user);
            }

            // TODO - handle on UI level the validation
            var errors = result
                .Errors
                .Select(x => new string[] { x.Description });

            object response = new
            {
                Errors = new { Password = errors },
                Status = 400
            };

            return new ObjectResult(response) { StatusCode = 400 };
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                ApplicationUser user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    string token = await userManager.GeneratePasswordResetTokenAsync(user);
                    string encodeToken = HttpUtility.UrlEncode(token);

                    string host = HttpContext.Request.Host.Value;
                    string scheme = HttpContext.Request.Scheme;

                    string link = scheme + "://www." + host + "/resetpassword/?token=" + encodeToken + "&e=" + model.Email;

                    //await emailService.SendForgotPasswordLink(user.Email, link);
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordViewModel model)
        {
            try
            {
                ApplicationUser user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return BadRequest();
                }

                IdentityResult resetPassResult = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (!resetPassResult.Succeeded)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }


        private async Task<OkObjectResult> ReturnAuthenticatedUserAsync(ApplicationUser user)
        {
            UserAuthenticatedViewModel userAuth = this.mapper.Map<UserAuthenticatedViewModel>(user);
            userAuth.Token = await GenerateJwtToken(user.Email, user);

            return Ok(userAuth);
        }

        private async Task<string> GenerateJwtToken(string email, ApplicationUser user)
        {
            var roles = await this.userManager.GetRolesAsync(user);
            string userInRoles = string.Join(",", roles);
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(nameof(ApplicationUser.Id).ToLowerInvariant(), user.Id.ToString()),
                new Claim(nameof(ApplicationUser.Picture).ToLowerInvariant(), user.Picture ?? ""),
                new Claim("permissions", userInRoles),
            };

            // Append all roles
            foreach (var r in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.JwtExpireDays));

            JwtSecurityToken token = new JwtSecurityToken(
                jwtSettings.JwtIssuer,
                jwtSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

    }
}
