using Fin.Application.DTOs;
using Fin.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Fin.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokensDto> GenerateTokensAsync(Guid userId, TokenSettingsDto tokenSettings);
        bool ValidateRefreshToken(User user, string tokenSecret);
    }
}
