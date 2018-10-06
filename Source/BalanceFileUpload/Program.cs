
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Constants;
using System.IO;
using System.Threading.Tasks;

namespace BalanceFileUpload
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                    config.AddJsonFile("autofac.json", optional: false, reloadOnChange: true);

                    // Load Azure KeyVault secrets
                    var builtConfig = config.Build();
                    config.AddAzureKeyVault(
                            $"https://{builtConfig[AppSettings.AzureKeyVaultName]}.vault.azure.net/",
                            builtConfig[AppSettings.AzureAdClientId],
                            builtConfig[AppSettings.AzureAdClientSecret]);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddAutofac();
                    AutoFacConfig.Initialize(context.Configuration);
                    services.AddSingleton<IHostedService, BackgroundWorker>();
                });
            await builder.RunConsoleAsync();
        }
    }
}
