using FinApi.Entities;
using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Threading.Tasks;

namespace FinApi.Services
{
    public interface ITokenService
    {
        Task<TokensDto> GenerateTokensAsync(Guid userId);
        bool ValidateRefreshToken(User user);
    }
}
