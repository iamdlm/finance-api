using FinApi.Interfaces;
using FinApi.Requests;
using FinApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthService tokenService;

        public AuthController(IUserService userService, IAuthService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest signupRequest)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage)).ToList();

                if (errors.Any())
                {
                    return BadRequest(new TokenResponse
                    {
                        Message = $"{string.Join(" ", errors)}"
                    });
                }
            }

            SignupResponse signupResponse = await userService.SignupAsync(signupRequest);

            if (signupResponse.StatusCode != HttpStatusCode.OK)
            {
                return UnprocessableEntity(signupResponse);
            }

            return Ok(signupResponse.Email);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Missing login details.");
            }

            TokenResponse loginResponse = await userService.LoginAsync(loginRequest);

            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                return Unauthorized(new
                {
                    loginResponse.Message
                });
            }

            return Ok(loginResponse);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken) || refreshTokenRequest.UserId == Guid.Empty)
            {
                return BadRequest(new TokenResponse
                {
                    Message = "Missing refresh token details"
                });
            }

            ValidateRefreshTokenResponse validateRefreshTokenResponse = await tokenService.ValidateRefreshTokenAsync(refreshTokenRequest);

            if (validateRefreshTokenResponse.StatusCode != HttpStatusCode.OK)
            {
                return UnprocessableEntity(validateRefreshTokenResponse);
            }

            Tuple<string, string> tokenResponse = await tokenService.GenerateTokensAsync(validateRefreshTokenResponse.UserId);

            return Ok(new { AccessToken = tokenResponse.Item1, Refreshtoken = tokenResponse.Item2 });
        }
    }
}
