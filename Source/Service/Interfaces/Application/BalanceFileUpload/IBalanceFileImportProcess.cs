using Shared.Model.ServerModel;
using System.Threading.Tasks;

namespace Service.Interfaces.Application.BalanceFileUpload
{
    public interface IBalanceFileImportProcess
    {
        Task ProcessAsync(FileUploadModel message);
    }
}
