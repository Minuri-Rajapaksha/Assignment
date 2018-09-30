using Data.DbFactory.Application;
using Data.Interfaces.Repositories;
using Data.Interfaces.UnitOfWork.Application;
using Data.Repository;
using Shared.Model.DB.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.UnitOfWork.Application
{
    public class UnitOfWorkApplication : UnitOfWorkBase<ApplicationContext>, IUnitOfWorkApplication
    {
        private IRepository<AccountPeriodBalance> _accountPeriodBalanceRepository;
        private IRepository<User> _userRepository;
        private IRepository<Account> _accountRepository;
        private IRepository<Period> _periodRepository;

        public UnitOfWorkApplication(ApplicationContext context) : base(context)
        {
        }

        public IRepository<AccountPeriodBalance> AccountPeriodBalances
        {
            get
            {
                return _accountPeriodBalanceRepository ?? (
                    _accountPeriodBalanceRepository = new Repository<AccountPeriodBalance>(this.Context, this.Context.AccountPeriodBalance));
            }
        }

        public IRepository<User> Users
        {
            get
            {
                return _userRepository ?? (
                    _userRepository = new Repository<User>(this.Context, this.Context.User));
            }
        }

        public IRepository<Account> Accounts
        {
            get
            {
                return _accountRepository ?? (
                    _accountRepository = new Repository<Account>(this.Context, this.Context.Account));
            }
        }

        public IRepository<Period> Periods
        {
            get
            {
                return _periodRepository ?? (
                    _periodRepository = new Repository<Period>(this.Context, this.Context.Period));
            }
        }
    }
}
