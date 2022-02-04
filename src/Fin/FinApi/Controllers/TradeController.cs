using FinApi.Entities;
using FinApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("portfolios/{portfolioId}/trades")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService tradeService;

        public TradeController(ITradeService tradeService)
        {
            this.tradeService = tradeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid portfolioId)
        {
            IEnumerable<TradeResponse> trades = await tradeService.GetAllByPortfolioIdAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), portfolioId);

            if(trades == null)
            {
                return NotFound(new
                {
                    Message = "The porfolio doesn't exist or access is denied."
                });
            }

            return Ok(trades);
        }

        [HttpGet("{tradeId}")]
        public async Task<IActionResult> Get([FromRoute] Guid portfolioId, Guid tradeId)
        {
            TradeResponse trade = await tradeService.GetAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), portfolioId, tradeId);

            if (trade == null)
            {
                return NotFound(new
                {
                    Message = "The entity doesn't exist or access is denied."
                });
            }

            return Ok(trade);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromRoute] Guid portfolioId, TradeRequest tradeRequest)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();

                if (errors.Any())
                {
                    return BadRequest(new
                    {
                        Message = $"{string.Join(" ", errors)}"
                    });
                }
            }

            Guid? tradeId = await tradeService.AddAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), portfolioId, tradeRequest);

            if (tradeId == null)
            {
                return BadRequest(new
                {
                    Message = "An error has occurred. Please try again."
                });
            }

            return Ok(tradeId);
        }

        [HttpDelete("{tradeId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid portfolioId, Guid tradeId)
        {
            bool result = await tradeService.DeleteAsync(new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)), portfolioId, tradeId);

            if (!result)
            {
                return NotFound(new
                {
                    Message = "The entity doesn't exist or access is denied."
                });
            }

            return NoContent();
        }
    }
}
