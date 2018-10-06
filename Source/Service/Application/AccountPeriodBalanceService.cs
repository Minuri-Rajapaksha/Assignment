using Data.Interfaces.DbFactory.Application;
using Data.Interfaces.File;
using Service.Interfaces.Application;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Model.ServerModel;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var randomFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            var fileUploadModel = new FileUploadModel
            {
                FileName = randomFileName,
                FileType = Shared.Enum.FileType.UploadDocument,
                Extension = Path.GetExtension(randomFileName),
                PeriodId = periodId
            };

            var fileUploadSuccess = await _fileAccessor.WriteFileAsync(stream, fileUploadModel);

            if (fileUploadSuccess)
            {                                
                await _importProcess.ProcessFileAsync(fileUploadModel);
            }

            return false;
        }
    }
}