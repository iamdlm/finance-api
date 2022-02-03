using FinApi.Entities;
using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinApi.Services
{
    public interface IUserService
    {
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<bool> SignupAsync(SignupRequest signupRequest);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserResponse>> GetAllAsync();
    }
}
