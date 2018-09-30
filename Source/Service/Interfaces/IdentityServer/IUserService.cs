using Shared.Model.DB.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.IdentityServer
{
    public interface IUserService
    {
        Task<User> GetAsync(string userName);

        Task<User> GetAsync(int userId);
    }
}
