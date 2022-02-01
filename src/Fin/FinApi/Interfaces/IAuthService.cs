using FinApi.Entities;
using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Threading.Tasks;

namespace FinApi.Interfaces
{
    public interface IAuthService
    {
        Task<Tuple<string, string>> GenerateTokensAsync(Guid userId);
        Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
        Task<bool> RemoveRefreshTokenAsync(User user);
    }
}
