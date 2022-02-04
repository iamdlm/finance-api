using FinApi.Services;
using FinApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace FinApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("portfolios")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            this.portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<PortfolioResponse> portfolios = await portfolioService.GetAllByUserIdAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return Ok(portfolios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            PortfolioResponse portfolio = await portfolioService.GetAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), id);

            if (portfolio == null)
                return NotFound("Portfolio doesn't exist or access is denied.");

            return Ok(portfolio);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(PortfolioRequest portfolioRequest)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();

                if (errors.Any())
                    return BadRequest(new
                    {
                        Message = $"{string.Join(" ", errors)}"
                    });
            }

            Guid? portfolioId = await portfolioService.AddAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), portfolioRequest);

            if (portfolioId == null)
                return BadRequest(new
                {
                    Message = "An error has occurred. Please try again."
                });

            return Ok(portfolioId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            bool result = await portfolioService.DeleteAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), id);

            if (!result)
                return NotFound("Portfolio doesn't exist or access is denied.");

            return NoContent();
        }

        [HttpGet("{id}/balance")]
        public IActionResult BalanceAsync(Guid id)
        {
            return Ok();
        }
    }
}
