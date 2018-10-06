
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interfaces.Application.BackgroundWorker
{
    public interface IBalanceFileUpload
    {
        Task RunAsync();
    }
}
