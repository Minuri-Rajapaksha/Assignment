using System;
using System.Threading.Tasks;

namespace Service.Interfaces.Application
{
    public interface ILogService
    {
        Task LogErrorAsync(Exception exception);
    }
}
