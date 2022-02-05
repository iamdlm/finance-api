using Fin.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Fin.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRepository<Portfolio> PortfolioRepository { get; }
        IRepository<Trade> TradeRepository { get; }
        Task<bool> CompleteAsync();
    }
}
