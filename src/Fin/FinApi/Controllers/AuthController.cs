using FinApi.Services;
using FinApi.Requests;
using FinApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FinApi.Helpers;
using FinApi.Entities;

namespace FinApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest signupRequest)
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

            User existingUser = await this.userService.GetUserByEmail(signupRequest.Email);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    Message = "The email address is already being used."
                });
            }

            if (signupRequest.Password != signupRequest.ConfirmPassword)
            {
                return BadRequest(new
                {
                    Message = "Password and confirm password do not match."
                });
            }

            if (PasswordHelper.IsValid(signupRequest.Password))
            {
                return BadRequest(new
                {
                    Message = "Password is weak."
                });
            }

            bool signupResult = await userService.SignupAsync(signupRequest);

            if (!signupResult)
            {
                return BadRequest(new
                {
                    Message = "An error has occurred. Please try again."
                });
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new
                {
                    Message = "Missing login details."
                });
            }

            TokenResponse tokenResponse = await userService.LoginAsync(loginRequest);

            if (tokenResponse == null)
            {
                return BadRequest(new
                {
                    Message = "Invalid login details."
                });
            }

            return Ok(tokenResponse);
        }
    }
}
