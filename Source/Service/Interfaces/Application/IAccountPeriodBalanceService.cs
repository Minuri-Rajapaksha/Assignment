using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.Application
{
    public interface IAccountPeriodBalanceService
    {
        Task<List<AccountPeriodBalanceModel>> GetAccountBalanceForPeriodAsync(int periodId);
    }
}
