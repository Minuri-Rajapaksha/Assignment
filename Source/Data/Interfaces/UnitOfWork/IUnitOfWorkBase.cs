using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkBase
    {
        Task SaveAsync();
    }
}
