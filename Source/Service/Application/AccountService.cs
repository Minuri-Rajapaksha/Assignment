﻿using Data.Interfaces.DbFactory.Application;
using Service.Interfaces.Application;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Application
{
    public class AccountService : IAccountService
    {
        private readonly IApplicationDbFactory _applicationDbFactory;

        public AccountService(IApplicationDbFactory applicationDbFactory)
        {
            _applicationDbFactory = applicationDbFactory;
        }

        public async Task<List<AccountModel>> GetAccountDropdownListAsync()
        {
            using (var uow = await _applicationDbFactory.BeginUnitOfWorkAsync())
            {
                return uow.Accounts.GetAll().Select(p => new AccountModel
                {
                    AccountId = p.AccountId,
                    AccountName = p.AccountName
                }).ToList();
            }
        }
    }
}
