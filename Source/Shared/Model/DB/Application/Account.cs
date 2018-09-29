using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class Account
    {
        public Account()
        {
            AccountBalance = new HashSet<AccountBalance>();
        }

        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }

        public User CreatedByNavigation { get; set; }
        public ICollection<AccountBalance> AccountBalance { get; set; }
    }
}
