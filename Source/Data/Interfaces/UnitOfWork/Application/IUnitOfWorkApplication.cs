using Data.Interfaces.Repositories;
using Shared.Model.DB.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interfaces.UnitOfWork.Application
{
    public interface IUnitOfWorkApplication : IUnitOfWorkBase, IDisposable
    {
        IRepository<AccountPeriodBalance> AccountPeriodBalances { get; }

        IRepository<User> Users { get; }

        IRepository<Account> Accounts { get; }

        IRepository<Period> Periods { get; }

    }
}
