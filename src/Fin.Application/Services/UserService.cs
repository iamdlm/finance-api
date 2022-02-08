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
using Fin.Domain.Exceptions;

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
                throw new NotFoundException("Entity doesn't exist or access is denied.");

            string passwordHash = PasswordHelper.HashUsingPbkdf2(loginRequest.Password, Convert.FromBase64String(user.Salt));

            if (user.Password != passwordHash)
                throw new BadRequestException("Wrong password.");

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
                Token = tokens.Token,
                TokenExpiration = tokens.TokenExpiration.ToString("yyyy-MM-ddTHH:mm:ssK"),
                RefreshToken = tokens.RefreshToken,
                RefreshTokenExpiration = tokens.RefreshTokenExpiration.ToString("yyyy-MM-ddTHH:mm:ssK")
            };
        }

        public async Task<UserResponse> SignupAsync(SignupRequest signupRequest)
        {
            UserResponse existingUser = await this.GetUserByEmailAsync(signupRequest.Email);

            if (existingUser != null)
                throw new BadRequestException("The email address is already being used.");

            if (signupRequest.Password != signupRequest.ConfirmPassword)
                throw new BadRequestException("Password and confirm password do not match.");

            if (PasswordHelper.IsValid(signupRequest.Password))
                throw new BadRequestException("Password is weak.");

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
                return null;

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
                throw new NotFoundException("Entity doesn't exist or access is denied.");

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
                throw new NotFoundException("Entity doesn't exist or access is denied.");

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
                throw new NotFoundException("Entity doesn't exist or access is denied.");

            bool result = tokenService.ValidateRefreshToken(user, tokenSettings.TokenSecret);

            if (!result)
                return null;

            TokensDto tokensDto = await tokenService.GenerateTokensAsync(refreshTokenDto.UserId, tokenSettings);

            return new TokenResponse()
            {
                Token = tokensDto.Token,
                TokenExpiration = tokensDto.TokenExpiration.ToString("yyyy-MM-ddTHH:mm:ssK"),
                RefreshToken = tokensDto.RefreshToken,
                RefreshTokenExpiration = tokensDto.RefreshTokenExpiration.ToString("yyyy-MM-ddTHH:mm:ssK")
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