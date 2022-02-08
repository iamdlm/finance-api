using Fin.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Text;
using Fin.Application.DTOs;
using Fin.Application.Interfaces;
using Fin.Application.Helpers;
using Fin.Domain.Exceptions;

namespace Fin.Application.Services
{
    public class TokenService : ITokenService
    {
        public TokenService() { }

        public async Task<TokensDto> GenerateTokensAsync(Guid userId, TokenSettingsDto tokenSettings)
        {
            DateTime accessTokenExpiration = DateTime.UtcNow.AddHours(2);

            string accessToken = TokenHelper.GenerateAccessToken(
                userId,
                tokenSettings.TokenIssuer,
                tokenSettings.TokenAudience,
                tokenSettings.TokenSecret,
                accessTokenExpiration);

            string refreshToken = await TokenHelper.GenerateRefreshToken();

            byte[] salt = Encoding.ASCII.GetBytes(tokenSettings.TokenSecret);

            string refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);

            return new TokensDto()
            {
                Token = accessToken,
                TokenExpiration = accessTokenExpiration,
                RefreshToken = refreshTokenHashed,
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(30)
            };
        }

        public bool ValidateRefreshToken(User user, string tokenSecret)
        {
            string refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(user.RefreshToken, Convert.FromBase64String(tokenSecret));

            if (user != null && user.RefreshToken != refreshTokenToValidateHash)
                throw new BadRequestException("Invalid refresh token.");

            if (user.RefreshTokenExpiration < DateTime.UtcNow)
                throw new BadRequestException("Refresh token expired.");

            return true;
        }
    }
}
