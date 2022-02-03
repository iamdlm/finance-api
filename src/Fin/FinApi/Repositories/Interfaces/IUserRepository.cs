using FinApi.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinApi.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
