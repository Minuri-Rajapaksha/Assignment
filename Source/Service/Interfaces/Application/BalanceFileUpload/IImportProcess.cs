
using Shared.Model.ServerModel;
using System.Threading.Tasks;

namespace Service.Interfaces.Application.BalanceFileUpload
{
    public interface IImportProcess
    {
        Task ProcessFileAsync(FileUploadModel message);
    }
}
