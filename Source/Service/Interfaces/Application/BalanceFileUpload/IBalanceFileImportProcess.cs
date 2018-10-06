using Shared.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.Application.BalanceFileUpload
{
    public interface IBalanceFileImportProcess
    {
        Task ProcessAsync(BalanceImportMessage message);
    }
}
