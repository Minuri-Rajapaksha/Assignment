using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/accountperiodbalance")]
    [ApiController]
    public class AccountPeriodBalanceController : ControllerBase
    {
        private readonly IAccountPeriodBalanceService _accountPeriodBalanceService;

        public AccountPeriodBalanceController(IAccountPeriodBalanceService accountPeriodBalanceService)
        {
            _accountPeriodBalanceService = accountPeriodBalanceService;
        }

        [HttpGet("{periodId}")]
        public async Task<List<AccountPeriodBalanceModel>> GetPeriodDropdownListAsync(int periodId)
        {
            return await _accountPeriodBalanceService.GetAccountBalanceForPeriodAsync(periodId);
        }
    }
}