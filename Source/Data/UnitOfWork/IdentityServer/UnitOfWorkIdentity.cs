using Data.DbFactory.IdentityServer;
using Data.Interfaces.Repositories;
using Data.Interfaces.UnitOfWork.IdentityServer;
using Data.Repository;
using Shared.Model.DB.Application;
using Shared.Model.DB.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.UnitOfWork.IdentityServer
{
    public class UnitOfWorkIdentity : UnitOfWorkBase<IdentityContext>, IUnitOfWorkIdentity
    {
        private IRepository<Clients> _clientsRepository;
        private IRepository<ApiResources> _apiResourcesRepository;
        private IRepository<IdentityResources> _identityResourcesRepository;
        private IRepository<User> _userRepository;

        public UnitOfWorkIdentity(IdentityContext context) : base(context)
        {
        }

        public IRepository<Clients> Clients
        {
            get
            {
                return _clientsRepository ?? (
                    _clientsRepository = new Repository<Clients>(this.Context, this.Context.Clients));
            }
        }

        public IRepository<ApiResources> ApiResources
        {
            get
            {
                return _apiResourcesRepository ?? (
                    _apiResourcesRepository = new Repository<ApiResources>(this.Context, this.Context.ApiResources));
            }
        }

        public IRepository<IdentityResources> IdentityResources
        {
            get
            {
                return _identityResourcesRepository ?? (
                    _identityResourcesRepository = new Repository<IdentityResources>(this.Context, this.Context.IdentityResources));
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
    }
}
