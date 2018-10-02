using Data.Interfaces.DbFactory.Application;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Application
{
    public class PeriodService : IPeriodService
    {
        private readonly IApplicationDbFactory _applicationDbFactory;

        public PeriodService(IApplicationDbFactory applicationDbFactory)
        {
            _applicationDbFactory = applicationDbFactory;
        }

        public async Task<List<PeriodModel>> GetPeriodDropdownListAsync()
        {
            using (var uow = await _applicationDbFactory.BeginUnitOfWorkAsync())
            {
                return uow.Periods.GetAll().Select(p => new PeriodModel
                {
                    PeriodId = p.PeriodId,
                    Discription = p.PeriodDate.Year + "-" + p.PeriodDate.Month
                }).ToList();
            }
        }
    }
}
