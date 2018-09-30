using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class Period
    {
        public Period()
        {
            AccountBalance = new HashSet<AccountPeriodBalance>();
        }

        public int PeriodId { get; set; }
        public string Discription { get; set; }
        public DateTime PeriodDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public ICollection<AccountPeriodBalance> AccountBalance { get; set; }
    }
}
