using FinApi.Entities;
using FinApi.Services;
using FinApi.Requests;
using FinApi.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FinApi.Repositories;

namespace FinApi.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IUnitOfWork unitOfWork;

        public PortfolioService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<AddPortfolioResponse> AddAsync(Guid userId, PortfolioRequest loginRequest)
        {
            User user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            Portfolio portfolio = new Portfolio()
            {
                Name = loginRequest.Name,
                User = user
            };

            unitOfWork.PortfolioRepository.Add(portfolio);

            bool saveResponse = await unitOfWork.CompleteAsync();

            if (!saveResponse)
            {
                return null;
            }

            return new AddPortfolioResponse
            {
                Id = portfolio.Id
            };
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid portfolioId)
        {
            Portfolio portfolio = await unitOfWork.PortfolioRepository.GetByIdAsync(portfolioId, t => t.Trades, u => u.User);

            if (portfolio == null || portfolio.User.Id != userId)
            {
                return false;
            }

            unitOfWork.PortfolioRepository.Delete(portfolio);

            return await unitOfWork.CompleteAsync();
        }

        public async Task<PortfolioResponse> GetAsync(Guid userId, Guid portfolioId)
        {
            Portfolio portfolio = await unitOfWork.PortfolioRepository.GetByIdAsync(portfolioId, t => t.Trades, u => u.User);

            if (portfolio == null || portfolio.User.Id != userId)
            {
                return null;
            }

            return new PortfolioResponse()
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                Trades = portfolio.Trades
            };
        }

        public async Task<IEnumerable<PortfolioResponse>> GetAllFromUserAsync(Guid userId)
        {
            IEnumerable<Portfolio> portfolios = await unitOfWork.PortfolioRepository.GetAllAsync(m => m.User.Id == userId, null, t => t.Trades, u => u.User);

            return portfolios.Select(s => new PortfolioResponse()
            {
                Id = s.Id,
                Name = s.Name,
                Trades = s.Trades
            });
        }
    }
}
