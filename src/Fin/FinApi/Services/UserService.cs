using Microsoft.EntityFrameworkCore;
using FinApi.Helpers;
using FinApi.Services;
using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Threading.Tasks;
using System.Linq;
using FinApi.Entities;
using System.Net;
using FinApi.Repositories;

namespace FinApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;

        public UserService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
        }

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest)
        {
            User user = await unitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                return null;
            }

            string passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.Salt));

            if (user.Password != passwordHash)
            {
                return null;
            }

            TokensDto tokens = await tokenService.GenerateTokensAsync(user.Id);

            user.Token = tokens.Token;
            user.TokenExpiration = tokens.TokenExpiration;
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiration = tokens.RefreshTokenExpiration;

            unitOfWork.UserRepository.Update(user);

            bool result = await unitOfWork.CompleteAsync();

            if (result)
            {
                return new TokenResponse
                {
                    AccessToken = tokens.Token,
                    RefreshToken = tokens.RefreshToken
                };
            }

            return null;
        }

        public async Task<bool> SignupAsync(SignupRequest signupRequest)
        {
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

            unitOfWork.UserRepository.Add(user);

            return await unitOfWork.CompleteAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await unitOfWork.UserRepository.GetByIdAsync(userId);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await unitOfWork.UserRepository.GetByEmailAsync(email);
        }

        public async Task<TokenResponse> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            User user = await unitOfWork.UserRepository.GetByIdAsync(refreshTokenDto.UserId);

            bool result = tokenService.ValidateRefreshToken(user);

            if (!result)
            {
                return null;
            }

            TokensDto tokensDto = await tokenService.GenerateTokensAsync(refreshTokenDto.UserId);

            return new TokenResponse()
            {
                AccessToken = tokensDto.Token,
                RefreshToken = tokensDto.RefreshToken
            };
        }
    }
}