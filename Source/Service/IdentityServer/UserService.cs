using Data.Interfaces.DbFactory.IdentityServer;
using Service.Interfaces.IdentityServer;
using Shared.Model.DB.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IdentityServer
{
    public class UserService : IUserService
    {
        private readonly IIdentityDbFactory _identityDbFactory;

        public UserService(IIdentityDbFactory identityDbFactory)
        {
            _identityDbFactory = identityDbFactory;
        }

        public async Task<User> GetAsync(string userName)
        {
            using (var uow = await _identityDbFactory.BeginUnitOfWorkAsync())
            {
                return uow.Users.Get().FirstOrDefault(u => u.UserName == userName);
            }
        }

        public async Task<User> GetAsync(int userId)
        {
            using (var uow = await _identityDbFactory.BeginUnitOfWorkAsync())
            {
                return uow.Users.Get().FirstOrDefault(u => u.UserId == userId);
            }
        }
    }
}
