using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {        
        }

        [HttpPost("signup")]
        public IActionResult Signup()
        {
            return this.Ok();
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            return this.Ok();
        }
    }
}
