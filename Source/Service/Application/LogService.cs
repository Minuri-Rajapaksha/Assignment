using Microsoft.Extensions.Logging;
using Service.Interfaces.Application;
using System;
using System.Threading.Tasks;

namespace Service.Application
{
    public class LogService : ILogService
    {
        private readonly ILogger _logger;

        public LogService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task LogErrorAsync(Exception exception)
        {
            _logger.LogError(exception, exception.Message);
        }

        public async Task LogInfoAsync(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
