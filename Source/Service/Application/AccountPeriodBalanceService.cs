using Data.Interfaces.File;
using Data.Interfaces.DbFactory.Application;
using OfficeOpenXml;
using Service.Interfaces.Application;
using Shared.Model.DB.Application;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Queue;
using Service.Interfaces.Application.BalanceFileUpload;

namespace Service.Application
{
    public class AccountPeriodBalanceService : IAccountPeriodBalanceService
    {
        private readonly IApplicationDbFactory _applicationDbFactory;
        private readonly IFileAccessor _fileAccessor;
        private readonly IImportProcess _importProcess;

        public AccountPeriodBalanceService(IApplicationDbFactory applicationDbFactory, IFileAccessor fileAccessor, IImportProcess importProcess)
        {
            _applicationDbFactory = applicationDbFactory;
            _fileAccessor = fileAccessor;
            _importProcess = importProcess;
        }

        public async Task<List<AccountPeriodBalanceModel>> GetAccountBalanceForPeriodAsync(int periodId)
        {
            using (var uow = await _applicationDbFactory.BeginUnitOfWorkAsync())
            {
                return uow.AccountPeriodBalances.GetAll().Where(a => a.PeriodId == periodId)
                    .Select(p => new AccountPeriodBalanceModel
                    {
                        AccountName = p.Account.AccountName,
                        Balance = p.Balance
                    }).ToList();
            }
        }

        public async Task<bool> UploadAndImportFile(int periodId, Stream stream, string fileName)
        {            
            var randomFileName = Path.Combine(Guid.NewGuid().ToString(), Path.GetExtension(fileName));

            var fileUploadSuccess = await _fileAccessor.UploadFileAsync(stream, fileName, "Result");
            if (fileUploadSuccess)
            {
                
                await _importProcess.ProcessFileAsync(new BalanceImportMessage
                {
                    FileName = randomFileName,
                    Extension = Path.GetExtension(fileName),
                    PeriodId = periodId
                });
            }

            return false;
        }
    }
}