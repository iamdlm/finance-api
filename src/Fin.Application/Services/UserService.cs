using Fin.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Linq;
using Fin.Domain.Repositories;
using System.Collections.Generic;
using Fin.Application.ViewModels;
using Fin.Application.DTOs;
using Fin.Application.Interfaces;
using Fin.Application.Helpers;

namespace Fin.Application.Services
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

        public async Task<TokenResponse> LoginAsync(LoginRequest loginRequest, TokenSettingsDto tokenSettings)
        {
            User user = await unitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email);

            if (user == null)
                return null;

            string passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.Salt));

            if (user.Password != passwordHash)
                return null;

            TokensDto tokens = await tokenService.GenerateTokensAsync(user.Id, tokenSettings);

            user.Token = tokens.Token;
            user.TokenExpiration = tokens.TokenExpiration;
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiration = tokens.RefreshTokenExpiration;

            unitOfWork.UserRepository.Update(user);

            bool result = await unitOfWork.CompleteAsync();

            if (!result)
                return null;

            return new TokenResponse
            {
                AccessToken = tokens.Token,
                RefreshToken = tokens.RefreshToken
            };
        }

        public async Task<UserResponse> SignupAsync(SignupRequest signupRequest)
        {
            UserResponse existingUser = await this.GetUserByEmailAsync(signupRequest.Email);

            if (existingUser != null)
                return new UserResponse()
                {
                    Message = "The email address is already being used."
                };

            if (signupRequest.Password != signupRequest.ConfirmPassword)
                return new UserResponse()
                {
                    Message = "Password and confirm password do not match."
                };

            if (PasswordHelper.IsValid(signupRequest.Password))
                return new UserResponse()
                {
                    Message = "Password is weak."
                };

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

            bool result = await unitOfWork.CompleteAsync();

            if (!result)
                return new UserResponse()
                {
                    Message = "An error has occurred."
                };

            return new UserResponse() 
            {
                Name = signupRequest.Name,
                Username = signupRequest.Username
            };
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid userId)
        {
            User user = await unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

            return new UserResponse()
            {
                Name = user.Name,
                Username = user.Username
            };
        }

        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            User user = await unitOfWork.UserRepository.GetByEmailAsync(email);

            if (user == null)
                return null;

            return new UserResponse()
            {
                Name = user.Name,
                Username = user.Username
            };
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, TokenSettingsDto tokenSettings)
        {
            User user = await unitOfWork.UserRepository.GetByIdAsync(refreshTokenDto.UserId);

            if (user == null)
                return null;

            bool result = tokenService.ValidateRefreshToken(user, tokenSettings.TokenSecret);

            if (!result)
                return null;

            TokensDto tokensDto = await tokenService.GenerateTokensAsync(refreshTokenDto.UserId, tokenSettings);

            return new TokenResponse()
            {
                AccessToken = tokensDto.Token,
                RefreshToken = tokensDto.RefreshToken
            };
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            IEnumerable<User> users = await unitOfWork.UserRepository.GetAllAsync();

            return users.Select(u => new UserResponse()
            {
                Name = u.Name,
                Username = u.Username
            });
        }
    }
}