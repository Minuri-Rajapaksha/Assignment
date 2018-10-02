using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/period")]
    [ApiController]
    public class PeriodController : ControllerBase
    {
        private readonly IPeriodService _periodService;

        public PeriodController(IPeriodService periodService)
        {
            _periodService = periodService;
        }

        [HttpGet("dropdownlist")]
        public async Task<List<PeriodModel>> GetPeriodDropdownListAsync()
        {
            return await _periodService.GetPeriodDropdownListAsync();
        }
    }
}