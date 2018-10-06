using Data.File.Interfaces;
using Data.Interfaces.DbFactory.Application;
using OfficeOpenXml;
using Service.Interfaces.Application;
using Shared.Model.DB.Application;
using Shared.Model.WebClientModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Application
{
    public class AccountPeriodBalanceService : IAccountPeriodBalanceService
    {
        private readonly IApplicationDbFactory _applicationDbFactory;
        private readonly IFileAccessor _fileAccessor;

        public AccountPeriodBalanceService(IApplicationDbFactory applicationDbFactory, IFileAccessor fileAccessor)
        {
            _applicationDbFactory = applicationDbFactory;
            _fileAccessor = fileAccessor;
        }

        public async Task<List<AccountPeriodBalanceModel>> GetAccountBalanceForPeriodAsync(int periodId)
        {
            using (var uow = await _applicationDbFactory.BeginUnitOfWorkAsync())
            {
                return uow.AccountPeriodBalances.GetAll().Where(a => a.PeriodId == periodId)
                    .Select(p => new AccountPeriodBalanceModel
                    {
                        AccountName = p.Account.AccountName,
                        Balance = p.Balance
                    }).ToList();
            }
        }

        public async Task<string> UploadAndImportFile(int periodId, Stream stream, string fileName)
        {
            StringBuilder responseMessage = new StringBuilder();
            var uploadSuccessModel = await _fileAccessor.UploadFileAsync(stream, fileName);
            if (uploadSuccessModel != null)
            {
                Dictionary<string, decimal> accountBalances = new Dictionary<string, decimal>();
                ExtractData(uploadSuccessModel, accountBalances);

                if (accountBalances != null)
                {
                    foreach (var item in accountBalances)
                    {
                        using (var uow = await _applicationDbFactory.BeginUnitOfWorkAsync())
                        {
                            var account = uow.Accounts.GetAll().FirstOrDefault(a => a.AccountName == item.Key);
                            if (account != null)
                            {
                                var balanceRecord = uow.AccountPeriodBalances.GetAll().FirstOrDefault(b => b.PeriodId == periodId && b.AccountId == account.AccountId);
                                if (balanceRecord == null)
                                {
                                    var accountBalance = new AccountPeriodBalance
                                    {
                                        AccountId = account.AccountId,
                                        Balance = item.Value,
                                        CreatedBy = 1,
                                        CreatedDate = DateTimeOffset.Now,
                                        PeriodId = periodId
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
            else
            {
                responseMessage.Append("File uploading error !" + System.Environment.NewLine);            
            }

            return responseMessage.ToString();
        }

        private void ExtractData(FileUploadModel uploadSuccessModel, Dictionary<string, decimal> accountBalances)
        {
            if (uploadSuccessModel.Extension == ".txt")
            {
                string[] content;
                content = File.ReadAllLines(uploadSuccessModel.FilePath);

                for (int i = 0; i < content.Length; i++)
                {
                    var row = content[i];
                    string[] rowArray = row.Split('\t');
                    accountBalances.Add(rowArray[0], decimal.Parse(rowArray[1]));
                }
            }
            else
            {
                FileInfo excelFile = new FileInfo(uploadSuccessModel.FilePath);

                using (ExcelPackage package = new ExcelPackage(excelFile))
                {                   
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        var rawText = string.Empty;
                        for (int col = 1; col <= ColCount; col++)
                        {
                            rawText += worksheet.Cells[row, col].Value.ToString() + "\t";
                        }
                        string[] rowArray = rawText.Split('\t');
                        accountBalances.Add(rowArray[0], decimal.Parse(rowArray[1]));
                    }
                }
            }
        }
    }
}