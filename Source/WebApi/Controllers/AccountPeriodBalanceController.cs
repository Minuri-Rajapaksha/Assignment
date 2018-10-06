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

        [HttpGet("{periodId}")]
        public async Task<List<AccountPeriodBalanceModel>> GetPeriodDropdownListAsync(int periodId)
        {
            return await _accountPeriodBalanceService.GetAccountBalanceForPeriodAsync(periodId);
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