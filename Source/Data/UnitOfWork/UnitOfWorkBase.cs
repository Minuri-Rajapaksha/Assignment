using Data.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWorkBase<TDbContext> : IUnitOfWorkBase where TDbContext : DbContext
    {
        protected readonly TDbContext Context;

        protected UnitOfWorkBase(TDbContext context)
        {
            Context = context;
        }

        public async Task SaveAsync()
        {
            await this.Context.SaveChangesAsync();
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
