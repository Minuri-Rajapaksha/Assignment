﻿using Data.Interfaces.DbFactory.Application;
using Data.Interfaces.File;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using Service.Interfaces.Application.BalanceFileUpload;
using Shared.Constants;
using Shared.Model.DB.Application;
using Shared.Model.ServerModel;
using Shared.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Application.BalanceFileUpload
{
    public class BalanceFileImportProcess : IBalanceFileImportProcess
    {
        private readonly IFileAccessor _fileAccessor;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbFactory _applicationDbFactory;

        public BalanceFileImportProcess(IFileAccessor fileAccessor, IConfiguration configuration, IApplicationDbFactory applicationDbFactory)
        {
            _fileAccessor = fileAccessor;
            _configuration = configuration;
            _applicationDbFactory = applicationDbFactory;
        }

        public async Task ProcessAsync(BalanceImportMessage message)
        {
            Dictionary<string, decimal> accountBalances = new Dictionary<string, decimal>();
            await ReadDataFromFile(message, accountBalances);
            var responseMessage = new StringBuilder();

            if (accountBalances != null)
            {
                foreach (var item in accountBalances)
                {
                    using (var uow = await _applicationDbFactory.BeginUnitOfWorkAsync())
                    {
                        var account = uow.Accounts.GetAll().FirstOrDefault(a => a.AccountName == item.Key);
                        if (account != null)
                        {
                            var balanceRecord = uow.AccountPeriodBalances.GetAll().FirstOrDefault(b => b.PeriodId == message.PeriodId && b.AccountId == account.AccountId);
                            if (balanceRecord == null)
                            {
                                var accountBalance = new AccountPeriodBalance
                                {
                                    AccountId = account.AccountId,
                                    Balance = item.Value,
                                    CreatedBy = 1,
                                    CreatedDate = DateTimeOffset.Now,
                                    PeriodId = message.PeriodId
                                };
                                uow.AccountPeriodBalances.Insert(accountBalance);
                                responseMessage.Append("Balance Inserted: " + item.Key + System.Environment.NewLine);
                            }
                            else
                            {
                                balanceRecord.Balance = item.Value;
                                balanceRecord.CreatedDate = DateTimeOffset.Now;
                                balanceRecord.CreatedBy = 1;
                                responseMessage.Append("Balance Updated: " + item.Key + System.Environment.NewLine);
                            }
                        }
                        else
                        {
                            responseMessage.Append(item.Key + " : Account does not exist !" + System.Environment.NewLine);
                        }
                        await uow.SaveAsync();
                    }
                }
            }
            else
            {
                responseMessage.Append("Data extraction error !" + System.Environment.NewLine);
            }
        }

        private async Task ReadDataFromFile(BalanceImportMessage message, Dictionary<string, decimal> accountBalances)
        {
            var content = await _fileAccessor.ReadFileAsync(new FileUploadModel {
                FileName = message.FileName,
                Extension = message.Extension,
                FileType = Shared.Enum.FileType.UploadDocument
            });

            for (int i = 0; i < content.Length; i++)
            {
                var row = content[i];
                string[] rowArray = row.Split('\t');
                accountBalances.Add(rowArray[0], decimal.Parse(rowArray[1]));
            }
        }
    }
}
