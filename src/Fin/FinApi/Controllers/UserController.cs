using FinApi.Entities;
using FinApi.Requests;
using FinApi.Responses;
using FinApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinApi.Controllers
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
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
            {
                return BadRequest(new
                {
                    Message = "Missing refresh token details."
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

            TokenResponse tokenResponse = await userService.RefreshToken(
                new RefreshTokenDto
                {
                    UserId = new Guid(userId),
                    RefreshToken = refreshTokenRequest.RefreshToken
                });

            if (tokenResponse == null)
            {
                return BadRequest(new
                {
                    Message = "Invalid refresh token."
                });
            }

            return Ok(new
            {
                AccessToken = tokenResponse.AccessToken,
                Refreshtoken = tokenResponse.RefreshToken
            });
        }
    }
}
