using System;
using System.Collections.Generic;

namespace Shared.Model.DB.Application
{
    public partial class User
    {
        public User()
        {
            Account = new HashSet<Account>();
            AccountBalance = new HashSet<AccountPeriodBalance>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }

        public ICollection<Account> Account { get; set; }
        public Role Role { get; set; }
        public ICollection<AccountPeriodBalance> AccountBalance { get; set; }
    }
}
