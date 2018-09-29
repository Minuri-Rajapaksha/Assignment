using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class Period
    {
        public Period()
        {
            AccountBalance = new HashSet<AccountBalance>();
        }

        public int PeriodId { get; set; }
        public string Discription { get; set; }
        public DateTime Period1 { get; set; }

        public ICollection<AccountBalance> AccountBalance { get; set; }
    }
}
