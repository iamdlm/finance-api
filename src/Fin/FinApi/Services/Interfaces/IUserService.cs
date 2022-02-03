using FinApi.Entities;
using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Threading.Tasks;

namespace FinApi.Services
{
    public interface IUserService
    {
        Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
        Task<bool> SignupAsync(SignupRequest signupRequest);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmail(string email);
        Task<TokenResponse> RefreshToken(RefreshTokenDto refreshTokenDto);
    }
}
