using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IHostingEnvironment _hostingEnvironment;

        public AccountPeriodBalanceController(IAccountPeriodBalanceService accountPeriodBalanceService, IHostingEnvironment hostingEnvironment)
        {
            _accountPeriodBalanceService = accountPeriodBalanceService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{periodId}")]
        public async Task<List<AccountPeriodBalanceModel>> GetPeriodDropdownListAsync(int periodId)
        {
            return await _accountPeriodBalanceService.GetAccountBalanceForPeriodAsync(periodId);
        }

        [HttpPost]
        public async Task<int> SaveUploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                //string webRootPath = _hostingEnvironment.WebRootPath;
                //string newPath = Path.Combine(webRootPath, folderName);
                //if (!Directory.Exists(newPath))
                //{
                //    Directory.CreateDirectory(newPath);
                //}
                //if (file.Length > 0)
                //{
                //    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //    string fullPath = Path.Combine(newPath, fileName);
                //    using (var stream = new FileStream(fullPath, FileMode.Create))
                //    {
                //        file.CopyTo(stream);
                //    }
                //}
                //return Json("Upload Successful.");
            }
            catch (Exception ex)
            {
                //return Json("Upload Failed: " + ex.Message);
            }

            //  return await _accountPeriodBalanceService.SaveUploadFile();
            return 1;
        }
    }
}