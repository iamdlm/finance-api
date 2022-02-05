using Fin.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fin.Application.Interfaces
{
    public interface IPortfolioService
    {
        Task<Guid?> AddAsync(Guid userId, PortfolioRequest loginRequest);
        Task<bool> DeleteAsync(Guid userId, Guid portfolioId);
        Task<PortfolioResponse> GetAsync(Guid userId, Guid portfolioId);
        Task<IEnumerable<PortfolioResponse>> GetAllByUserIdAsync(Guid userId);
    }
}
