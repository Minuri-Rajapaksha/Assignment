using Data.Interfaces.Repositories;
using Shared.Model.DB.Application;
using Shared.Model.DB.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interfaces.UnitOfWork.IdentityServer
{
    public interface IUnitOfWorkIdentity : IUnitOfWorkBase, IDisposable
    {
        IRepository<Clients> Clients { get; }

        IRepository<ApiResources> ApiResources { get; }

        IRepository<IdentityResources> IdentityResources { get; }

        IRepository<User> Users { get; }
    }
}
