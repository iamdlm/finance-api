using Fin.Application.DTOs;
using Fin.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fin.Application.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest, TokenSettingsDto tokenSettings);
        Task<UserResponse> SignupAsync(SignupRequest signupRequest);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto, TokenSettingsDto tokenSettings);
        Task<UserResponse> GetUserByIdAsync(Guid userId);
        Task<UserResponse> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserResponse>> GetAllAsync();
    }
}
