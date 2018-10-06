using Shared.Model.WebClientModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Service.Interfaces.Application
{
    public interface IAccountPeriodBalanceService
    {
        Task<List<AccountPeriodBalanceModel>> GetAccountBalanceForPeriodAsync(int periodId);

        Task<string> UploadAndImportFile(int periodId, Stream stream, string fileName);
    }
}
