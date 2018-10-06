
using Data.Interfaces.Queue;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Model.ServerModel;
using System.Threading.Tasks;

namespace Service.Application.BalanceFileUpload
{
    public class QueueImportProcess : IImportProcess
    {
        private readonly IQueueAccessor<FileUploadModel> _queueAccessor;

        public QueueImportProcess(IQueueAccessor<FileUploadModel> queueAccessor)
        {
            _queueAccessor = queueAccessor;
        }

        public async Task ProcessFileAsync(FileUploadModel message)
        {
            await _queueAccessor.SendAsync(message);
        }
    }
}
