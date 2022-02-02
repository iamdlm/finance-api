using Microsoft.EntityFrameworkCore;
using FinApi.Helpers;
using FinApi.Interfaces;
using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Threading.Tasks;
using System.Linq;
using FinApi.Entities;
using System.Net;

namespace FinApi.Services
{
    public class UserService : IUserService
    {
        private readonly FinDbContext dbContext;
        private readonly IAuthService tokenService;

        public UserService(FinDbContext _dbContext, IAuthService tokenService)
        {
            this.dbContext = _dbContext;
            this.tokenService = tokenService;
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            User user = dbContext.Users.SingleOrDefault(user => user.Email == loginRequest.Email);

            if (user == null)
            {
                return new TokenResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Email not found"
                };
            }

            string passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.Salt));

            if (user.Password != passwordHash)
            {
                return new TokenResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid Password"
                };
            }

            Tuple<string, string> token = await tokenService.GenerateTokensAsync(user.Id);

            return new TokenResponse
            {
                StatusCode = HttpStatusCode.OK,
                AccessToken = token.Item1,
                RefreshToken = token.Item2
            };
        }

        public async Task<SignupResponse> SignupAsync(SignupRequest signupRequest)
        {
            User existingUser = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == signupRequest.Email);

            if (existingUser != null)
            {
                return new SignupResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "The email address is already being used."
                };
            }

            if (signupRequest.Password != signupRequest.ConfirmPassword) {
                return new SignupResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Password and confirm password do not match."
                };
            }

            if (PasswordHelper.IsValid(signupRequest.Password))
            {
                return new SignupResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Password is weak."
                };
            }

            byte[] salt = PasswordHelper.GetSecureSalt();
            string passwordHash = PasswordHelper.HashUsingPbkdf2(signupRequest.Password, salt);

            User user = new User
            {
                Email = signupRequest.Email,
                Password = passwordHash,
                Salt = Convert.ToBase64String(salt),
                Name = signupRequest.Name,
                Username = signupRequest.Username
            };

            await dbContext.Users.AddAsync(user);

            int saveResponse = await dbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new SignupResponse 
                { 
                    StatusCode = HttpStatusCode.OK, 
                    Email = user.Email,
                    Username = user.Username,
                    Name = user.Name
                };
            }

            return new SignupResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Unable to save the user"
            };
        }
    }
}