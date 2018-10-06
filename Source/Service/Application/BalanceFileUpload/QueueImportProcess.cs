
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Queue;
using System.Threading.Tasks;

namespace Service.Application.BalanceFileUpload
{
    public class QueueImportProcess : IImportProcess
    {
        public async Task ProcessFileAsync(BalanceImportMessage message)
        {

        }
    }
}
