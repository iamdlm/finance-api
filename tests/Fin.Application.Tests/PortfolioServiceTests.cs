using Fin.Application.Services;
using Fin.Application.ViewModels;
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
    public class PortfolioServiceTests : IClassFixture<InMemoryTestFixture>
    {
        public readonly InMemoryTestFixture fixture;
        public readonly UnitOfWork unitOfWork;
        public readonly PortfolioService portfolioService;

        public PortfolioServiceTests(InMemoryTestFixture dbContext)
        {
            this.fixture = dbContext;
            this.unitOfWork = new UnitOfWork(fixture.DbContext);
            this.portfolioService = new PortfolioService(unitOfWork);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Portfolio()
        {
            // Act
            var portfolio = await portfolioService.GetAsync(MockData.UserB.Id, MockData.PortfolioB2.Id);

            // Assert
            Assert.NotNull(portfolio);
        }

        [Fact]
        public async Task GetByIdAsync_Throws_NotFoundException()
        {
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await portfolioService.GetAsync(MockData.UserB.Id, new Guid()));
        }

        [Fact]
        public async Task GetAllByUserIdAsync_Returns_Portfolios()
        {
            // Act
            var portfolio = await portfolioService.GetAllByUserIdAsync(MockData.UserB.Id);

            // Assert
            Assert.NotNull(portfolio);
        }

        [Fact]
        public async Task GetAllByUserIdAsync_Returns_Empty()
        {
            // Act
            var portfolio = await portfolioService.GetAllByUserIdAsync(new Guid());

            // Assert
            Assert.NotNull(portfolio);
            Assert.Empty(portfolio);
        }

        [Fact]
        public async Task AddAsync_Increases_Count()
        {
            // Arrange
            PortfolioRequest portfolioRequest = new PortfolioRequest()
            {
                Name = "PB3"
            };

            // Act
            var portfoliosBefore = await portfolioService.GetAllByUserIdAsync(MockData.UserB.Id);
            var portfolio = await portfolioService.AddAsync(MockData.UserB.Id, portfolioRequest);
            var portfoliosAfter = await portfolioService.GetAllByUserIdAsync(MockData.UserB.Id);

            // Assert
            Assert.NotNull(portfolio);
            Assert.NotEmpty(portfoliosBefore);
            Assert.NotEmpty(portfoliosAfter);
            Assert.Equal(portfoliosBefore.ToList().Count + 1, portfoliosAfter.ToList().Count);
        }

        [Fact]
        public async Task DeleteAsync_Decreases_Count()
        {
            // Act            
            var portfoliosBefore = await portfolioService.GetAllByUserIdAsync(MockData.UserB.Id);
            var result = await portfolioService.DeleteAsync(MockData.UserB.Id, MockData.PortfolioB1.Id);
            var portfoliosAfter = await portfolioService.GetAllByUserIdAsync(MockData.UserB.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(portfoliosBefore.ToList().Count - 1, portfoliosAfter.ToList().Count);
        }

        [Fact]
        public async Task DeleteAsync_Throws_NotFoundException()
        {
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => portfolioService.DeleteAsync(MockData.UserB.Id, new Guid()));
        }

        [Fact]
        public async Task DeleteAsync_Throws_BadRequestException()
        {
            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => portfolioService.DeleteAsync(MockData.UserB.Id, MockData.PortfolioB2.Id));
        }

        [Fact]
        public async Task GetBalanceAsync_WithNoTrades_Returns_Zero()
        {
            // Act
            var balance = await portfolioService.GetBalanceAsync(MockData.UserB.Id, MockData.PortfolioB4.Id);

            // Assert
            Assert.NotNull(balance);
            Assert.Equal(0, balance.Balance);
        }

        [Fact]
        public async Task GetBalanceAsync_WithOneTrade_Returns_Positive()
        {
            // Act
            var balance = await portfolioService.GetBalanceAsync(MockData.UserB.Id, MockData.PortfolioB2.Id);

            // Assert
            Assert.NotNull(balance);
            Assert.Equal(70000, balance.Balance);
        }

        [Fact]
        public async Task GetBalanceAsync_WithMultipleTrades_Returns_Positive()
        {
            // Act
            var balance = await portfolioService.GetBalanceAsync(MockData.UserB.Id, MockData.PortfolioB3.Id);

            // Assert
            Assert.NotNull(balance);
            Assert.Equal(8500, balance.Balance);
        }
    }
}
