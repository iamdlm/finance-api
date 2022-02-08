using Fin.Application.DTOs;
using Fin.Application.Services;
using Fin.Application.ViewModels;
using Fin.Domain.Entities;
using Fin.Domain.Exceptions;
using Fin.Domain.Tests;
using Fin.Infrastructure.Repositories;
using Fin.Infrastructure.Tests;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fin.Application.Tests
{
    public class TradeServiceTests : IClassFixture<InMemoryTestFixture>
    {
        public readonly InMemoryTestFixture fixture;
        public readonly UnitOfWork unitOfWork;
        public readonly TradeService tradeService;

        public TradeServiceTests(InMemoryTestFixture dbContext)
        {
            this.fixture = dbContext;
            this.unitOfWork = new UnitOfWork(fixture.DbContext);
            this.tradeService = new TradeService(unitOfWork);
        }

        [Fact]
        public async Task GetAsync_Returns_Trade()
        {
            // Act
            var trade = await tradeService.GetAsync(MockData.UserB.Id, MockData.PortfolioB2.Id, MockData.TradePB2.Id);

            // Assert
            Assert.NotNull(trade);
            Assert.IsAssignableFrom<TradeResponse>(trade);
        }

        [Fact]
        public async Task GetAsync_Throws_NotFoundException()
        {
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await tradeService.GetAsync(new Guid(), new Guid(), new Guid()));
        }

        [Fact]
        public async Task GetAllByPortfolioIdAsync_Returns_Trades()
        {
            // Act
            var trades = await tradeService.GetAllByPortfolioIdAsync(MockData.UserB.Id, MockData.PortfolioB2.Id);

            // Assert
            Assert.NotNull(trades);
            Assert.NotEmpty(trades);
        }

        [Fact]
        public async Task GetAllByPortfolioIdAsync_Throws_NotFoundException()
        {
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await tradeService.GetAllByPortfolioIdAsync(new Guid(), new Guid()));
        }

        [Fact]
        public async Task AddAsync_Returns_Guid()
        {
            // Arrange
            TradeRequest tradeRequest = new TradeRequest()
            {
                Action = "buy",
                Asset = "BTC",
                Currency = "EUR",
                Date = "2022-07-02",
                MarketValue = 100,
                NumberOfShares = 10,
                Price = 10,
                Notes = string.Empty
            };

            // Act
            var guid = await tradeService.AddAsync(MockData.UserB.Id, MockData.PortfolioB2.Id, tradeRequest);

            // Assert
            Assert.NotNull(guid);
            Assert.IsAssignableFrom<Guid>(guid);
        }

        [Fact]
        public async Task AddAsync_Throws_BadRequestException_InvalidAction()
        {
            // Arrange
            TradeRequest tradeRequest = new TradeRequest()
            {
                Action = "wrong",
                Asset = "BTC",
                Currency = "EUR",
                Date = "2022-07-02",
                MarketValue = 100,
                NumberOfShares = 10,
                Price = 10,
                Notes = string.Empty
            };

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await tradeService.AddAsync(MockData.UserB.Id, MockData.PortfolioB2.Id, tradeRequest));
        }

        [Fact]
        public async Task AddAsync_Throws_BadRequestException_InvalidDate()
        {
            // Arrange
            TradeRequest tradeRequest = new TradeRequest()
            {
                Action = "buy",
                Asset = "BTC",
                Currency = "EUR",
                Date = "2022-aa-02",
                MarketValue = 100,
                NumberOfShares = 10,
                Price = 10,
                Notes = string.Empty
            };

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await tradeService.AddAsync(MockData.UserB.Id, MockData.PortfolioB2.Id, tradeRequest));
        }
    }
}
