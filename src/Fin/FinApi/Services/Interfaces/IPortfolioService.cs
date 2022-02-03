using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinApi.Services
{
    public interface IPortfolioService
    {
        Task<AddPortfolioResponse> AddAsync(Guid userId, PortfolioRequest loginRequest);
        Task<bool> DeleteAsync(Guid userId, Guid portfolioId);
        Task<PortfolioResponse> GetAsync(Guid userId, Guid portfolioId);
        Task<IEnumerable<PortfolioResponse>> GetAllFromUserAsync(Guid userId);
    }
}
