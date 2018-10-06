
using Data.Interfaces.Queue;
using Service.Interfaces.Application.BackgroundWorker;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Model.ServerModel;
using System.Threading.Tasks;

namespace Service.Application.BackgroundWorker
{
    public class BalanceFileUpload : IBalanceFileUpload
    {
        private readonly IQueueAccessor<FileUploadModel> _queueAccessor;
        private readonly IBalanceFileImportProcess _balanceFileImportProcess;

        public BalanceFileUpload(IQueueAccessor<FileUploadModel> queueAccessor, IBalanceFileImportProcess balanceFileImportProcess)
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
