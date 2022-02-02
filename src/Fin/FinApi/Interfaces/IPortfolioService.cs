using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Threading.Tasks;

namespace FinApi.Interfaces
{
    public interface IPortfolioService
    {
        Task<AddPortfolioResponse> AddAsync(Guid userId, PortfolioRequest loginRequest);
        Task DeleteAsync(Guid portfolioId);
        Task<PortfolioResponse> GetAsync(Guid portfolioId);
    }
}
