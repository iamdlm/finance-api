using Fin.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Fin.Application.Interfaces;
using Fin.Application.ViewModels;
using Fin.Application.Helpers;
using Fin.Application.DTOs;
using Fin.Api.Helpers;
using Microsoft.Extensions.Options;

namespace FinApi.Controllers
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

            UserResponse existingUser = await this.userService.GetUserByEmailAsync(signupRequest.Email);

            if (existingUser != null)
                return BadRequest(new
                {
                    Message = "The email address is already being used."
                });

            if (signupRequest.Password != signupRequest.ConfirmPassword)
                return BadRequest(new
                {
                    Message = "Password and confirm password do not match."
                });

            if (PasswordHelper.IsValid(signupRequest.Password))
                return BadRequest(new
                {
                    Message = "Password is weak."
                });

            bool signupResult = await userService.SignupAsync(signupRequest);

            if (!signupResult)
                return BadRequest(new
                {
                    Message = "An error has occurred. Please try again."
                });

            return Ok();
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

            return Ok(new
            {
                AccessToken = tokenResponse.AccessToken,
                Refreshtoken = tokenResponse.RefreshToken
            });
        }
    }
}
