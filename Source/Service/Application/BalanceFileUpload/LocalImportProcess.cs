
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Model.ServerModel;
using System.Threading.Tasks;

namespace Service.Application.BalanceFileUpload
{
    public class LocalImportProcess : IImportProcess
    {
        private readonly IBalanceFileImportProcess _balanceFileImportProcess;

        public LocalImportProcess(IBalanceFileImportProcess balanceFileImportProcess)
        {
            _balanceFileImportProcess = balanceFileImportProcess;
        }

        public async Task ProcessFileAsync(FileUploadModel message)
        {
            await _balanceFileImportProcess.ProcessAsync(message);
        }
    }
}
