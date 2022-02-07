using Fin.Domain.Entities;
using Fin.Domain.Tests;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fin.Infrastructure.Tests
{
    public class InMemoryTestFixture : IDisposable
    {
        public FinDbContext DbContext { get; private set; }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }

        public InMemoryTestFixture()
        {
            Init();
        }

        private void Init()
        {
            var options = new DbContextOptionsBuilder<FinDbContext>()
                .UseInMemoryDatabase("FinDbContext")
                .Options;

            DbContext = new FinDbContext(options);

            Populate();

            DbContext.SaveChanges();
        }

        private void Populate()
        {
            DbContext.Database.EnsureDeleted();

            PopulateUserData();
            PopulatePortfolioData();
            PopulateTradeData();
        }

        private void PopulateUserData()
        {
            DbContext.Set<User>().AddRange(MockData.Users);
        }
        
        private void PopulatePortfolioData()
        {
            DbContext.Set<Portfolio>().AddRange(MockData.Portfolios);
        }

        private void PopulateTradeData()
        {
            DbContext.Set<Trade>().AddRange(MockData.Trades);
        }
    }
}
