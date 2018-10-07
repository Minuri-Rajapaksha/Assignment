using Data.Interfaces.DbFactory.Application;
using Data.Interfaces.File;
using Service.Interfaces.Application;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Model.ServerModel;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            var randomFileName = Guid.NewGuid().ToString();
            var fileUploadModel = new FileUploadModel
            {
                FileName = randomFileName,
                FileType = Shared.Enum.FileType.UploadDocument,
                Extension = Path.GetExtension(fileName),
                PeriodId = periodId
            };

            var fileUploadSuccess = await _fileAccessor.WriteFileAsync(stream, fileUploadModel);

            if (fileUploadSuccess)
            {
                await _importProcess.ProcessFileAsync(fileUploadModel);
            }

            return false;
        }

        public async Task<AccountPeriodBalanceReportModel> GetAccountBalanceForPeriodRangeAsync(AccountBalancePeriodRangeModel accPeriodBal)
        {
            var returnVal = new AccountPeriodBalanceReportModel();
            using (var uow = await _applicationDbFactory.BeginUnitOfWorkAsync())
            {
                var periodStartDate = uow.Periods.GetAll().First(i => i.PeriodId == accPeriodBal.StartPeriodId).PeriodDate;
                var periodEndDate = uow.Periods.GetAll().First(i => i.PeriodId == accPeriodBal.EndPeriodId).PeriodDate;
                var accounts = uow.Accounts.GetAll().Where(i => i.AccountId == accPeriodBal.AccountId || accPeriodBal.AccountId == 0)
                    .Select(i => new { AccountId = i.AccountId, AccountName = i.AccountName }).OrderBy(i => i.AccountName).ToList();
                var periods = uow.Periods.GetAll().Where(i => i.PeriodDate >= periodStartDate && i.PeriodDate <= periodEndDate)
                    .Select(i => new { PeriodId = i.PeriodId, PeriodDate = i.PeriodDate, Discription = i.Discription }).
                    OrderBy(i => i.PeriodDate).ToList();
                var accountPeriodBalance = uow.AccountPeriodBalances.GetAll()
                    .Where(i => accounts.Select(a => a.AccountId).Contains(i.AccountId) &&
                        periods.Select(p => p.PeriodId).Contains(i.PeriodId))
                    .Select(i => new { PeriodId = i.PeriodId, AccountId = i.AccountId, Balance = i.Balance }).ToList();

                Random rnd = new Random();
                returnVal.Period = periods.Select(i => i.Discription).ToList();
                foreach (var account in accounts)
                {
                    var accBal = new ChartDataSet
                    {
                        Data = new List<decimal>(),
                        BorderColor = "#" + Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)).Name,
                        Label = account.AccountName
                    };

                    foreach (var period in periods)
                    {
                        var amount = accountPeriodBalance.FirstOrDefault(i => i.AccountId == account.AccountId && i.PeriodId == period.PeriodId);
                        if (amount == null)
                        {
                            accBal.Data.Add(0);
                        }
                        else
                        {
                            accBal.Data.Add(amount.Balance);
                        }
                    }

                    returnVal.DataSet.Add(accBal);
                }

                return returnVal;
            }
        }
    }
}