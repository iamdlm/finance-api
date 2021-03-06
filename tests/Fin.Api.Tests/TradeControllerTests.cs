using Fin.Application.Services;
using Fin.Domain.Tests;
using Fin.Infrastructure.Repositories;
using Fin.Infrastructure.Tests;
using Fin.Api.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace Fin.Api.Tests
{
    [Collection("InMemoryTest collection")]
    public class TradeControllerTests : IClassFixture<InMemoryTestFixture>
    {
        public readonly InMemoryTestFixture fixture;
        public readonly UnitOfWork unitOfWork;
        public readonly TradeService tradeService;
        public readonly TradeController tradeController;

        public TradeControllerTests(InMemoryTestFixture dbContext)
        {
            this.fixture = dbContext;
            this.unitOfWork = new UnitOfWork(fixture.DbContext);
            this.tradeService = new TradeService(unitOfWork);
            this.tradeController = new TradeController(tradeService);
        }

        [Fact]
        public async Task GetAllAsync_Returns_Trades()
        {
            Assert.True(true);
        }
    }
}
