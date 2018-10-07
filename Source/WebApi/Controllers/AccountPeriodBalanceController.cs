using Data.Interfaces.File;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/accountperiodbalance")]
    [ApiController]
    public class AccountPeriodBalanceController : ControllerBase
    {
        private readonly IAccountPeriodBalanceService _accountPeriodBalanceService;

        public AccountPeriodBalanceController(IAccountPeriodBalanceService accountPeriodBalanceService, IFileAccessor fileAccessor)
        {
            _accountPeriodBalanceService = accountPeriodBalanceService;
        }

        [HttpGet("{periodid}")]
        public async Task<List<AccountPeriodBalanceModel>> GetAccountBalanceForPeriodAsync(int periodid)
        {
            return await _accountPeriodBalanceService.GetAccountBalanceForPeriodAsync(periodid);
        }

        [HttpGet("{accountid}/{startperiodid}/{endperiodid}")]
        public async Task<List<AccountPeriodBalanceReportModel>> GetAccountBalanceForPeriodRangeAsync(int accountid, int startperiodid, int endperiodid)
        {
            return await _accountPeriodBalanceService.GetAccountBalanceForPeriodRangeAsync(new AccountBalancePeriodRangeModel
            {
                AccountId = accountid,
                StartPeriodId = startperiodid,
                EndPeriodId = endperiodid
            });
        }

        [HttpPost]
        public async Task<bool> SaveUploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var periodId = Request.Form["PERIOD"][0];
                return await _accountPeriodBalanceService.UploadAndImportFile(Int32.Parse(periodId), file.OpenReadStream(), file.FileName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}