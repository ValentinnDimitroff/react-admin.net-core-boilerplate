using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using RaNetCore.Models.UserModels.Enums;
using RaNetCore.Services.BaseServices.Interfaces;
using RaNetCore.Services.BaseServices.Models;

namespace RaNetCore.Services.BaseServices
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HttpContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.User = new UserHttpContext()
            {
                UserIdStr = this.GetCurrenUserId(),
                UserId = this.GetCurrentUserIdParsed(),
                Username = this.GetCurrentUserUsername(),
                Roles = this.GetUserRoles()
            };
        }

        public UserHttpContext User { get; private set; }


        // Private Getter Methods
        private string GetCurrenUserId()
        {
            string userStrId = this.httpContextAccessor
                ?.HttpContext
                ?.User
                ?.FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

            return userStrId;
        }

        private int GetCurrentUserIdParsed()
        {
            string userStrId = this.GetCurrenUserId();

            int.TryParse(userStrId, out int userId);

            return userId;
        }

        private string GetCurrentUserUsername()
        {
            return this.httpContextAccessor
                ?.HttpContext
                ?.User
                ?.Identity
                ?.Name ?? "Non-Existing";
        }

        private IEnumerable<UserRoles> GetUserRoles()
        {
            IEnumerable<UserRoles> result = this.httpContextAccessor
                ?.HttpContext
                ?.User
                ?.FindFirst("permissions")
                ?.Value
                ?.Split(';')
                .Select(c => (UserRoles)Enum.Parse(typeof(UserRoles), c));

            return result;
        }
    }
}
