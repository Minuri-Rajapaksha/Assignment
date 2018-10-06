
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Queue;
using System.Threading.Tasks;

namespace Service.Application.BalanceFileUpload
{
    public class LocalImportProcess : IImportProcess
    {
        public async Task ProcessFile(BalanceImportMessage message)
        {

        }
    }
}
