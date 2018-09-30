using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Extensions
{
    public static class ContentSecurityPolicy
    {
        public static void ConfigureSecurityHeaders(this IApplicationBuilder app, IConfiguration config, IHostingEnvironment env)
        {
            const string fakeNonceHeaderKey = "X-CommonSecurityHeaders-Nonce";

            app.Use(async (context, next) =>
            {
                var request = context?.Request;
                if (request != null)
                {
                    context.Request.Headers.Add(
                        new KeyValuePair<string, StringValues>(
                            fakeNonceHeaderKey,
                            new StringValues(Convert.ToBase64String(Encoding.ASCII.GetBytes(Guid.NewGuid().ToString())).Substring(0, 10)))
                    );

                    await next();
                }
            });
        }
    }
}
