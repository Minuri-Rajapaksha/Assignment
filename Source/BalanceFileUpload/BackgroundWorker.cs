
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
        private readonly ILogService _logService;
        private readonly IBalanceFileUpload _balanceFileUpload;

        public BackgroundWorker(ILogService logService, IBalanceFileUpload balanceFileUpload)
        {
            _logService = logService;
            _balanceFileUpload = balanceFileUpload;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await _balanceFileUpload.RunAsync();
                }
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(ex);
                throw ex;
            }
        }
    }
}
