using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;

namespace WebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("dropdownlist")]
        public async Task<List<AccountModel>> GetAccountDropdownListAsync()
        {
            return await _accountService.GetAccountDropdownListAsync();
        }
    }
}