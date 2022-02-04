using FinApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Guid?> AddAsync(Guid userId, PortfolioRequest portfolioRequest)
        {
            User user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

            Portfolio portfolio = new Portfolio()
            {
                Name = portfolioRequest.Name,
                User = user
            };

            unitOfWork.PortfolioRepository.Add(portfolio);

            bool saveResponse = await unitOfWork.CompleteAsync();

            if (!saveResponse)
                return null;

            return portfolio.Id;
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid portfolioId)
        {
            Portfolio portfolio = await unitOfWork.PortfolioRepository.GetByIdAsync(portfolioId, u => u.User);

            if (portfolio == null || portfolio.User == null || portfolio.User.Id != userId)            
                return false;

            unitOfWork.PortfolioRepository.Delete(portfolio);

            return await unitOfWork.CompleteAsync();
        }

        public async Task<PortfolioResponse> GetAsync(Guid userId, Guid portfolioId)
        {
            Portfolio portfolio = await unitOfWork.PortfolioRepository.GetByIdAsync(portfolioId, u => u.User);

            if (portfolio == null || portfolio.User == null || portfolio.User.Id != userId)
                return null;

            return new PortfolioResponse()
            {
                Id = portfolio.Id,
                Name = portfolio.Name
            };
        }

        public async Task<IEnumerable<PortfolioResponse>> GetAllByUserIdAsync(Guid userId)
        {
            IEnumerable<Portfolio> portfolios = await unitOfWork.PortfolioRepository.GetAllAsync(p => p.User.Id == userId, null, u => u.User);

            return portfolios.Select(p => new PortfolioResponse()
            {
                Id = p.Id,
                Name = p.Name
            });
        }
    }
}
