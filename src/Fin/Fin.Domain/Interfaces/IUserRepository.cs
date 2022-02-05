using Fin.Domain.Entities;
using System.Threading.Tasks;

namespace Fin.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
