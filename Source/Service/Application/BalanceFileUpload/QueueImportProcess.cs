
using Data.Interfaces.Queue;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Queue;
using System.Threading.Tasks;

namespace Service.Application.BalanceFileUpload
{
    public class QueueImportProcess : IImportProcess
    {
        private readonly IQueueAccessor<BalanceImportMessage> _queueAccessor;

        public async Task ProcessFileAsync(BalanceImportMessage message)
        {
            await _queueAccessor.SendAsync(message);
        }
    }
}
