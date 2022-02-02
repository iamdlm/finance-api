using FinApi.Entities;
using FinApi.Interfaces;
using FinApi.Requests;
using FinApi.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinApi.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly FinDbContext dbContext;

        public PortfolioService(FinDbContext _DbContext)
        {
            dbContext = _DbContext;
        }

        public async Task<AddPortfolioResponse> AddAsync(Guid userId, PortfolioRequest loginRequest)
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(m => m.Id == userId);

            Portfolio portfolio = new Portfolio()
            {
                Name = loginRequest.Name,
                User = user
            };

            await dbContext.Portfolios.AddAsync(portfolio);

            int saveResponse = await dbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new AddPortfolioResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Id = portfolio.Id
                };
            }

            return new AddPortfolioResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to create the portfolio."
            };
        }

        public async Task DeleteAsync(Guid portfolioId)
        {
            throw new NotImplementedException();
        }

        public async Task<PortfolioResponse> GetAsync(Guid portfolioId)
        {
            throw new NotImplementedException();
        }
    }
}
