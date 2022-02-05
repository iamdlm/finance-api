using Fin.Domain.Entities;
using Fin.Domain.Repositories;
using System.Threading.Tasks;

namespace Fin.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinDbContext context;
        private readonly IUserRepository userRepository;
        private readonly IRepository<Portfolio> portfolioRepository;
        private readonly IRepository<Trade> tradeRepository;

        public UnitOfWork(FinDbContext context) => this.context = context;

        public IUserRepository UserRepository => userRepository ?? new UserRepository(context);

        public IRepository<Portfolio> PortfolioRepository => portfolioRepository ?? new Repository<Portfolio>(context);

        public IRepository<Trade> TradeRepository => tradeRepository ?? new Repository<Trade>(context);

        public async Task<bool> CompleteAsync() => await context.SaveChangesAsync() > 0;

        public void Dispose() => context.Dispose();
    }
}
