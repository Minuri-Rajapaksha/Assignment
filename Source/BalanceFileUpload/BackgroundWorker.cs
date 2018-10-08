
using Microsoft.Extensions.Hosting;
using Service.Interfaces.Application;
using Service.Interfaces.Application.BackgroundWorker;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BalanceFileUpload
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly IBalanceFileUpload _balanceFileUpload;

        public BackgroundWorker()
        {
            _balanceFileUpload = AutoFacConfig.Resolve<IBalanceFileUpload>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _balanceFileUpload.RunAsync();

                while (!stoppingToken.IsCancellationRequested)
                {
                    // service keep alive 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
