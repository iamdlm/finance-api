using FinApi.Entities;
using System;
using System.Threading.Tasks;

namespace FinApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRepository<Portfolio> PortfolioRepository { get; }
        IRepository<Trade> TradeRepository { get; }
        Task<bool> CompleteAsync();
    }
}
