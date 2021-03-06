﻿using Shared.Model.WebClientModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Service.Interfaces.Application
{
    public interface IAccountPeriodBalanceService
    {
        Task<List<AccountPeriodBalanceModel>> GetAccountBalanceForPeriodAsync(int periodId);

        Task<bool> UploadAndImportFile(int periodId, Stream stream, string fileName, int userId);

        Task<AccountPeriodBalanceReportModel> GetAccountBalanceForPeriodRangeAsync(AccountBalancePeriodRangeModel accountPeriodBalanceRange);
    }
}
