
using Data.Interfaces.Queue;
using Service.Interfaces.Application.BackgroundWorker;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Queue;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Application.BackgroundWorker
{
    public class BalanceFileUpload : IBalanceFileUpload
    {
        private readonly IQueueAccessor<BalanceImportMessage> _queueAccessor;
        private readonly IBalanceFileImportProcess _balanceFileImportProcess;

        public BalanceFileUpload(IQueueAccessor<BalanceImportMessage> queueAccessor, IBalanceFileImportProcess balanceFileImportProcess)
        {
            _queueAccessor = queueAccessor;
            _balanceFileImportProcess = balanceFileImportProcess;
        }

        public Task RunAsync()
        {
            return _queueAccessor.Receive(async balanceFileImportInfo =>
            {
                await _balanceFileImportProcess.ProcessAsync(balanceFileImportInfo);
            });
        }
    }
}
