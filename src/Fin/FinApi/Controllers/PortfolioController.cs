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
    [Route("portfolios")]
    public class PortfolioController : ControllerBase
    {
        public PortfolioController()
        {        
        }

        [HttpGet]
        public IActionResult GetAll(Guid id)
        {
            return this.Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return this.Ok();
        }

        [HttpPost]
        public IActionResult Add()
        {
            return this.Ok();
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
