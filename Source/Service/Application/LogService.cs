using Service.Interfaces.Application;
using System;
using System.Threading.Tasks;

namespace Service.Application
{
    public class LogService : ILogService
    {
        public async Task LogErrorAsync(Exception exception)
        {

        }
    }
}
