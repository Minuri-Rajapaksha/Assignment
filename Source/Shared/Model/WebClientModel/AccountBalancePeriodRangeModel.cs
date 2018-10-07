using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model.WebClientModel
{
    public class AccountBalancePeriodRangeModel
    {
        public int StartPeriodId { get; set; }
        public int EndPeriodId { get; set; }
        public int AccountId { get; set; }
    }
}
