using Autofac;
using Autofac.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Service.Interfaces.IdentityServer;
using Shared.Constants;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer
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
            // Enforce HTTPS
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            IdentityModelEventSource.ShowPII = true;

            // Add MVC Services and Web API
            services
                .AddMvc()
                .AddSessionStateTempDataProvider();

            // Add Cors
            var corsAllowOrigins = Configuration.GetValue<string>(AppSettings.AllowCors);
            services.AddCors(options =>
            {
                options.AddPolicy("SessionAliveOrigins",
                    builder => builder.WithOrigins(corsAllowOrigins)
                                        .WithMethods("POST"));
            });

            // Configuring Session
            services.AddMemoryCache();

            // Add session
            services.AddSession(options =>
            {
                options.Cookie.Name = "Auth.Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            // Configuring IdentityServer 4
            var connectionString = Configuration.GetConnectionString(ConnectionStrings.IdentityConnection);
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var idsBuilder = services.AddIdentityServer(options => { options.Csp.Level = IdentityServer4.Models.CspLevel.One; })
                .AddProfileService<ProfileService>()
                .AddJwtBearerClientAuthentication()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600;
                });

            // Signing Certificate for IdentityServer 4
            idsBuilder.AddDeveloperSigningCredential();

            services.AddAuthentication("AuthCookie")
                .AddCookie("AuthCookie", options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = "assignmentauth";

                    // Add Cookie expiration based on user 
                    options.Events.OnSigningIn = (context) =>
                    {
                        context.Properties.IsPersistent = false;
                        context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30);
                        return Task.CompletedTask;
                    };

                    // Turn sliding expiration on so we can keep alive the session by pinging from other SPs
                    options.SlidingExpiration = true;
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Exception handling configuration must be the first in the middleware pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler($"/Error/Index");
            }

            // Redirect if Not HTTPS
            var rewriteOptions = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(rewriteOptions);

            app.UseSession();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            app.ApplicationServices.GetService<IDatabaseInitializerService>().EnsureMigrationAsync().Wait();

            app.ApplicationServices.GetService<IDatabaseInitializerService>().SeedDataAsync().Wait();

            var sslPort = 0;
            app.Use(async (context, next) =>
            {
                if (context.Request.IsHttps)
                {
                    await next();
                }
                else
                {
                    var sslPortStr = sslPort == 0 || sslPort == 443 ? string.Empty : $":{sslPort}";
                    var httpsUrl = $"https://{context.Request.Host.Host}{sslPortStr}{context.Request.Path}";
                    context.Response.Redirect(httpsUrl);
                }
            });


            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Route not found");
            });
        }
    }
}
