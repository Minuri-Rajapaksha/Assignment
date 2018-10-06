using Data.File.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
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
        public async Task<string> SaveUploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                var periodId = Request.Form["PERIOD"][0];
                var result = await _accountPeriodBalanceService.UploadAndImportFile(Int32.Parse(periodId), file.OpenReadStream(), file.FileName);

                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return ("Upload Failed: " + ex.Message);
            }
        }
    }
}