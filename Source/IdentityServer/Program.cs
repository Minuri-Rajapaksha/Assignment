using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Shared.Constants;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(service => service.AddAutofac())
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;
                    var builtConfig = config.Build();

                    // Load Azure KeyVault secrets
                    if (env.IsProduction())
                    {
                        config.AddAzureKeyVault(
                            $"https://{builtConfig[AppSettings.AzureKeyVaultName]}.vault.azure.net/",
                            builtConfig[AppSettings.AzureAdClientId],
                            builtConfig[AppSettings.AzureAdClientSecret]);
                    }
                });
    }
}
