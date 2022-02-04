using FinApi.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinApi.Services
{
    public interface ITradeService
    {
        Task<Guid?> AddAsync(Guid userId, Guid portfolioId, TradeRequest tradeRequest);
        Task<bool> DeleteAsync(Guid userId, Guid portfolioId, Guid tradeId);
        Task<TradeResponse> GetAsync(Guid userId, Guid portfolioId, Guid tradeId);
        Task<IEnumerable<TradeResponse>> GetAllByPortfolioIdAsync(Guid userId, Guid portfolioId);
    }
}
