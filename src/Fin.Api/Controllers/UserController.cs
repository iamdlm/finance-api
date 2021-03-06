using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fin.Application.Interfaces;
using Fin.Application.ViewModels;

namespace Fin.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<UserResponse> users = await userService.GetAllAsync();

            return Ok(users);
        }
    }
}
