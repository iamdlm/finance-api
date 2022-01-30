using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("portfolios/{portfolioId}/trades")]
    public class TradeController : ControllerBase
    {
        public TradeController()
        {        
        }

        [HttpGet]
        public IActionResult GetAll([FromRoute] Guid portfolioId)
        {
            return this.Ok();
        }

        [HttpGet("{tradeId}")]
        public IActionResult Get([FromRoute] Guid portfolioId, Guid tradeId)
        {            return this.Ok();
        }

        [HttpPost]
        public IActionResult Add([FromRoute] Guid portfolioId)
        {
            return this.Ok();
        }

        [HttpDelete("{tradeId}")]
        public IActionResult Delete([FromRoute] Guid portfolioId, Guid tradeId)
        {
            return this.Ok();
        }
    }
}
