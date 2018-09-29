using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class AccountBalance
    {
        public int AccountBalanceId { get; set; }
        public int AccountId { get; set; }
        public int PeriodId { get; set; }
        public int CreatedBy { get; set; }
        public decimal Balance { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }

        public Account Account { get; set; }
        public User CreatedByNavigation { get; set; }
        public Period Period { get; set; }
    }
}
