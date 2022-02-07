using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Fin.Application.Interfaces;
using Fin.Application.ViewModels;
using Fin.Application.DTOs;
using Fin.Api.Helpers;
using Microsoft.Extensions.Options;

namespace Fin.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly AppSettings appSettings;

        public AuthController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            this.userService = userService;
            this.appSettings = appSettings.Value;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignupAsync(SignupRequest signupRequest)
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

            UserResponse signupResult = await userService.SignupAsync(signupRequest);

            if (signupResult == null)
                return BadRequest(new
                {
                    Message = signupResult.Message
                });

            return Ok(signupResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
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

            TokenResponse tokenResponse = await userService.LoginAsync(
                loginRequest,
                new TokenSettingsDto
                {
                    TokenIssuer = appSettings.TokenIssuer,
                    TokenAudience = appSettings.TokenAudience,
                    TokenSecret = appSettings.TokenSecret
                });

            if (tokenResponse == null)
                return BadRequest(new
                {
                    Message = "Invalid login details."
                });

            return Ok(tokenResponse);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
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

            TokenResponse tokenResponse = await userService.RefreshTokenAsync(
                new RefreshTokenDto
                {
                    UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    RefreshToken = refreshTokenRequest.RefreshToken
                },
                new TokenSettingsDto
                {
                    TokenIssuer = appSettings.TokenIssuer,
                    TokenAudience = appSettings.TokenAudience,
                    TokenSecret = appSettings.TokenSecret                    
                });

            if (tokenResponse == null)
                return BadRequest(new
                {
                    Message = "Invalid refresh token."
                });

            return Ok(tokenResponse);
        }
    }
}
