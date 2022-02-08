using Fin.Application.Interfaces;
using Fin.Application.ViewModels;
using Fin.Domain.Entities;
using Fin.Domain.Enums;
using Fin.Domain.Exceptions;
using Fin.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fin.Application.Services
{
    public class TradeService : ITradeService
    {
        private readonly IUnitOfWork unitOfWork;

        public TradeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid?> AddAsync(Guid userId, Guid portfolioId, TradeRequest tradeRequest)
        {
            User user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("Entity doesn't exist or access is denied.");

            Portfolio portfolio = await unitOfWork.PortfolioRepository.GetByIdAsync(portfolioId, u => u.User, t => t.Trades);

            if (portfolio == null || portfolio.User == null || portfolio.User.Id != userId)
                throw new NotFoundException("Entity doesn't exist or access is denied.");

            if (portfolio.Trades.Any(t => t.Currency != tradeRequest.Currency))
                throw new BadRequestException("Invalid currency for the specified portfolio.");

            DateTime date;
            TradeAction action;

            try
            {
                date = DateTime.Parse(tradeRequest.Date);
                bool conversion = Enum.TryParse(tradeRequest.Action, true, out action);
                if (!conversion)
                    throw new ArgumentException();
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                if (ex is ArgumentNullException || ex is FormatException)
                    message = "Invalid date.";

                if (ex is ArgumentException)
                    message = "Invalid action.";

                throw new BadRequestException(message);
            }

            Trade trade = new Trade()
            {
                Date = date,
                Action = action,
                Asset = tradeRequest.Asset,
                Currency = tradeRequest.Currency,
                MarketValue = tradeRequest.MarketValue,
                NumberOfShares = tradeRequest.NumberOfShares,
                Notes = tradeRequest.Notes,
                Price = tradeRequest.Price,
                User = user,
                Portfolio = portfolio
            };

            unitOfWork.TradeRepository.Add(trade);

            bool result = await unitOfWork.CompleteAsync();

            if (!result)
                return null;

            return trade.Id;
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid portfolioId, Guid tradeId)
        {
            Trade trade = await unitOfWork.TradeRepository.GetByIdAsync(tradeId);

            if (trade == null || trade.User.Id != userId || trade.Portfolio.Id != portfolioId)
                throw new NotFoundException("Entity doesn't exist or access is denied.");

            unitOfWork.TradeRepository.Delete(trade);

            return await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<TradeResponse>> GetAllByPortfolioIdAsync(Guid userId, Guid portfolioId)
        {
            Portfolio portfolio = await unitOfWork.PortfolioRepository.GetByIdAsync(portfolioId, u => u.User, t => t.Trades);

            if (portfolio == null || portfolio.User.Id != userId)
                throw new NotFoundException("Entity doesn't exist or access is denied.");

            return portfolio.Trades.Select(t => new TradeResponse()
            {
                Id = t.Id,
                Created = t.Created.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                Modified = t.Modified.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                Date = t.Date.ToString("yyyy-MM-dd"),
                Action = t.Action.ToString().ToLowerInvariant(),
                Asset = t.Asset,
                Currency = t.Currency,
                MarketValue = t.MarketValue,
                NumberOfShares = t.NumberOfShares,
                Notes = t.Notes,
                Price = t.Price,
                UserId = t.User.Id,
                PortfolioId = t.Portfolio.Id
            });
        }

        public async Task<TradeResponse> GetAsync(Guid userId, Guid portfolioId, Guid tradeId)
        {
            Trade trade = await unitOfWork.TradeRepository.GetByIdAsync(tradeId, u => u.User, p => p.Portfolio);

            if (trade == null || trade.User.Id != userId || trade.Portfolio.Id != portfolioId)
                throw new NotFoundException("Entity doesn't exist or access is denied.");

            return new TradeResponse()
            {
                Id = trade.Id,
                Created = trade.Created.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                Modified = trade.Modified.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                Date = trade.Date.ToString("yyyy-MM-dd"),
                Action = trade.Action.ToString().ToLowerInvariant(),
                Asset = trade.Asset,
                Currency = trade.Currency,
                MarketValue = trade.MarketValue,
                NumberOfShares = trade.NumberOfShares,
                Notes = trade.Notes,
                Price = trade.Price,
                UserId = trade.User.Id,
                PortfolioId = trade.Portfolio.Id
            };
        }
    }
}
