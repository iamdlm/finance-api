using Fin.Domain.Entities;
using Fin.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fin.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FinDbContext context) : base(context) { }

        public Task<User> GetByEmailAsync(string email) => context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
