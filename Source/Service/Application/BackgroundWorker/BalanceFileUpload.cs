
using Data.Interfaces.Queue;
using Service.Interfaces.Application.BackgroundWorker;
using Shared.Queue;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Application.BackgroundWorker
{
    public class BalanceFileUpload : IBalanceFileUpload
    {
        private readonly IQueueAccessor<BalanceImportMessage> _queueAccessor;

        public BalanceFileUpload(IQueueAccessor<BalanceImportMessage> queueAccessor)
        {
            _queueAccessor = queueAccessor;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {

        }
    }
}
