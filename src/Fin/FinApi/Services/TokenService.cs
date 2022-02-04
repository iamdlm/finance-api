using FinApi.Helpers;
using FinApi.Entities;
using System;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Options;

namespace FinApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings appSettings;

        public TokenService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public async Task<TokensDto> GenerateTokensAsync(Guid userId)
        {
            DateTime accessTokenExpiration = DateTime.Now.AddHours(2);

            string accessToken = TokenHelper.GenerateAccessToken(
                userId, 
                appSettings.TokenIssuer, 
                appSettings.TokenAudience, 
                appSettings.TokenSecret, 
                accessTokenExpiration);

            string refreshToken = await TokenHelper.GenerateRefreshToken();

            byte[] salt = Encoding.ASCII.GetBytes(appSettings.TokenSecret);

            string refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);

            return new TokensDto()
            {
                Token = accessToken,
                TokenExpiration = accessTokenExpiration,
                RefreshToken = refreshTokenHashed,
                RefreshTokenExpiration = DateTime.Now.AddDays(30)
            };
        }

        public bool ValidateRefreshToken(User user)
        {
            string refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(user.RefreshToken, Convert.FromBase64String(appSettings.TokenSecret));

            if (user.RefreshToken != refreshTokenToValidateHash)
            {
                return false;
            }
          
            if (user.RefreshTokenExpiration < DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }
}
