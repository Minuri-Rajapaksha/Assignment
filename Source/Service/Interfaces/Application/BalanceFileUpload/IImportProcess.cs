
using Shared.Queue;
using System.Threading.Tasks;

namespace Service.Interfaces.Application.BalanceFileUpload
{
    public interface IImportProcess
    {
        Task ProcessFileAsync(BalanceImportMessage message);
    }
}
