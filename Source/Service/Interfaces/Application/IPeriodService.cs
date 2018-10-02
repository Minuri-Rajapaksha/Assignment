using Shared.Model.WebClientModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces.Application
{
    public interface IPeriodService
    {
        Task<List<PeriodModel>> GetPeriodDropdownListAsync();
    }
}
