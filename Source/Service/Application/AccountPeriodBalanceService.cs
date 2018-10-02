using Data.Interfaces.DbFactory.Application;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Application
{
    public class AccountPeriodBalanceService : IAccountPeriodBalanceService
    {
        private readonly IApplicationDbFactory _applicationDbFactory;

        public AccountPeriodBalanceService(IApplicationDbFactory applicationDbFactory)
        {
            _applicationDbFactory = applicationDbFactory;
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
    }
}