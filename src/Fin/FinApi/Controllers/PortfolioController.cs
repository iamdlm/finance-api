using FinApi.Services;
using FinApi.Requests;
using FinApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public async Task<IActionResult> GetAll()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new
                {
                    Message = "Invalid request."
                });
            }

            IEnumerable<PortfolioResponse> portfolios = await portfolioService.GetAllFromUserAsync(new Guid(userId));

            return Ok(portfolios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new
                {
                    Message = "Invalid request."
                });
            }

            PortfolioResponse portfolio = await portfolioService.GetAsync(new Guid(userId), id);

            if (portfolio == null)
            {
                return Forbid("Portfolio doesn't exist or you don't have access.");
            }

            return Ok(portfolio);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] PortfolioRequest portfolioRequest)
        {
            if (portfolioRequest == null || string.IsNullOrEmpty(portfolioRequest.Name))
            {
                return BadRequest(new
                {
                    Message = "Missing portfolio details."
                });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new
                {
                    Message = "Invalid request."
                });
            }

            AddPortfolioResponse portfolioResponse = await portfolioService.AddAsync(new Guid(userId), portfolioRequest);

            if (portfolioResponse == null)
            {
                return BadRequest(new 
                {
                    Message = "An error has occurred. Please try again."
                });
            }

            return Ok(portfolioResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    Message = "Invalid portfolio Id."
                });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new
                {
                    Message = "Invalid request."
                });
            }

            bool result = await portfolioService.DeleteAsync(new Guid(userId), id);

            if (!result)
            {
                return Forbid("Portfolio doesn't exist or you don't have access.");
            }

            return Ok();
        }

        [HttpGet("{id}/balance")]
        public IActionResult Balance(Guid id)
        {
            return Ok();
        }
    }
}
