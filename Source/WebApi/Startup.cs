using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Interfaces.Application;
using Shared.Constants;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Security;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // Configure autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.ContentRootPath)
                .AddJsonFile(Environment.IsDevelopment() ? "autofac.Development.json" : "autofac.json", optional: false, reloadOnChange: true)
                .Build();

            // Register the ConfigurationModule with Autofac.
            builder.RegisterModule(new ConfigurationModule(config));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();

            services.AddMemoryCache();

            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter(
                        new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build()
                    )
                );
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);            

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.Authority = Configuration.GetValue<string>(AppSettings.IdentityServerHost);
                    options.ApiName = "webapi";
                    options.ApiSecret = "4h6KMinuri@#";
                    options.JwtBearerEvents.OnTokenValidated = OnTokenValidated;
                });

            var corsAllowOrigins = Configuration.GetValue<string>(AppSettings.AllowCors);
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(corsAllowOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Register the Swagger generator, defining one or more Swagger documents  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Assignment API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("default");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.ApplicationServices.GetService<IDatabaseInitializerService>().EnsureMigrationAsync().Wait();

            app.ApplicationServices.GetService<IDatabaseInitializerService>().SeedDataAsync().Wait();

            // Enable middleware to serve generated Swagger as a JSON endpoint.  
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assignment API V1");
            });
        }

        private Task OnTokenValidated(TokenValidatedContext context)
        {
            // Add the access_token as a claim, as we need it to call UserInfo endpoint in claims transformation
            if (context.SecurityToken is JwtSecurityToken accessToken && context.Principal.Identity is ClaimsIdentity identity)
            {
                identity.AddClaim(new Claim(ClaimType.AccessToken, accessToken.RawData));
            }

            return Task.CompletedTask;
        }
    }
}
