using FinApi.Interfaces;
using FinApi.Requests;
using FinApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("portfolios")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService portfolioService;

        public PortfolioController(IPortfolioService _portfolioService)
        {
            portfolioService = _portfolioService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return this.Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] PortfolioRequest portfolioRequest)
        {
            if (portfolioRequest == null || string.IsNullOrEmpty(portfolioRequest.Name))
            {
                return BadRequest(new AddPortfolioResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Missing portfolio details."
                });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            AddPortfolioResponse portfolioResponse = await portfolioService.AddAsync(new Guid(userId), portfolioRequest);

            if (portfolioResponse.StatusCode != HttpStatusCode.OK)
            {
                return UnprocessableEntity(portfolioResponse);
            }

            return Ok(portfolioResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return this.Ok();
        }

        [HttpGet("{id}/balance")]
        public IActionResult Balance(Guid id)
        {
            return this.Ok();
        }
    }
}
