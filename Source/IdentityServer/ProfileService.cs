using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Service.Interfaces.IdentityServer;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userService;

        public ProfileService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();

            if (context.Caller == IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint)
            {
                var user = await _userService.GetAsync(Int32.Parse(context.Subject.FindFirst(JwtClaimTypes.Subject).Value));
                claims.Add(new Claim(ClaimType.Role, user.RoleId.ToString()));
            }

            context.IssuedClaims = claims;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}
