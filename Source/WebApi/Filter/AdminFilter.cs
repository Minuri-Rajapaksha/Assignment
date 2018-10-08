using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Filter
{
    public class AdminFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IEnumerable<Claim> claims = ((ClaimsIdentity)context.HttpContext.User.Identity).Claims;
            
            if (claims.First(c => c.Type == "Role").Value != ((Int32)Shared.Enum.Role.Admin).ToString())
            {
                throw new UnauthorizedAccessException();
            }
            await next();
        }
    }
}
