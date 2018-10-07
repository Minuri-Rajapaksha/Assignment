using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Security
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public ClaimsTransformer(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var access_token = principal.FindFirst(ClaimType.AccessToken)?.Value;
            var jwtId = principal.FindFirst(JwtClaimTypes.JwtId)?.Value;

            if (String.IsNullOrEmpty(jwtId) || String.IsNullOrEmpty(access_token))
            {
                throw new UnauthorizedAccessException("JwtId or access token is missing");
            }

            // Cache this result
            if (_memoryCache.Get<ClaimsPrincipal>("profileCache") == null)
            {
                _memoryCache.Set("profileCache", await BuildClaimsPrincipal(access_token, principal), TimeSpan.FromMinutes(30));
            }
            var claimsPrincipalLite = _memoryCache.Get<ClaimsPrincipal>("profileCache");


            var claims = claimsPrincipalLite.Claims.Select(x => new Claim(x.Type, x.Value));
            var id = new ClaimsIdentity(claims, principal.Identity.AuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(id);

            // We need to add JwtId because TransformAsync can be called multiple times. so we need to add JwtId claim since its not in IAuthorizationContext
            (claimsPrincipal.Identity as ClaimsIdentity).AddClaim(new Claim(JwtClaimTypes.JwtId, jwtId));

            return claimsPrincipal;
        }

        private async Task<ClaimsPrincipal> BuildClaimsPrincipal(string accessToken, ClaimsPrincipal principal)
        {
            var discoveryClient = new DiscoveryClient(_configuration.GetValue<string>(AppSettings.IdentityServerHost));
            var doc = await discoveryClient.GetAsync();

            if (doc.IsError)
            {
                throw new UnauthorizedAccessException("Cannot find Login server");
            }

            var userInfoClient = new UserInfoClient(doc.UserInfoEndpoint);
            var response = await userInfoClient.GetAsync(accessToken);

            // Invalid access token
            if (response.IsError)
            {
                throw new UnauthorizedAccessException("Access token is not valid");
            }

            if (!long.TryParse(principal.FindFirstValue(JwtClaimTypes.Expiration), out var exp))
            {
                throw new UnauthorizedAccessException("Expiration cannot be found in provided JWT token");
            }

            ((ClaimsIdentity)principal.Identity).AddClaims(response.Claims);
            return principal;
        }
    }
}
