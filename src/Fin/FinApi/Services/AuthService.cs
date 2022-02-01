using Microsoft.EntityFrameworkCore;
using FinApi.Helpers;
using FinApi.Interfaces;
using FinApi.Requests;
using FinApi.Responses;
using System;
using System.Threading.Tasks;
using FinApi.Entities;
using System.Text;
using Microsoft.Extensions.Options;
using System.Net;

namespace FinApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly FinDbContext dbContext;
        private readonly AppSettings appSettings;

        public AuthService(FinDbContext _DbContext, IOptions<AppSettings> _appSettings)
        {
            dbContext = _DbContext;
            appSettings = _appSettings.Value;
        }

        public async Task<Tuple<string, string>> GenerateTokensAsync(Guid userId)
        {
            DateTime accessTokenExpiration = DateTime.Now.AddHours(2);

            string accessToken = TokenHelper.GenerateAccessToken(
                userId, 
                appSettings.TokenIssuer, 
                appSettings.TokenAudience, 
                appSettings.TokenSecret, 
                accessTokenExpiration);

            string refreshToken = await TokenHelper.GenerateRefreshToken();

            User userRecord = await dbContext.Users.FirstOrDefaultAsync(e => e.Id == userId);

            if (userRecord == null)
            {
                return null;
            }

            byte[] salt = Encoding.ASCII.GetBytes(appSettings.TokenSecret);

            string refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);

            userRecord.Token = accessToken;
            userRecord.TokenExpiration = accessTokenExpiration;
            userRecord.RefreshToken = refreshTokenHashed;
            userRecord.RefreshTokenExpiration = DateTime.Now.AddDays(30);
            userRecord.Salt = Convert.ToBase64String(salt);

            await dbContext.SaveChangesAsync();

            Tuple<string, string> token = new Tuple<string, string>(accessToken, refreshToken);

            return token;
        }

        public async Task<bool> RemoveRefreshTokenAsync(User user)
        {
            User userRecord = await dbContext.Users.FirstOrDefaultAsync(e => e.Id == user.Id);

            if (userRecord == null)
            {
                return false;
            }

            userRecord.RefreshToken = string.Empty;
            userRecord.RefreshTokenExpiration = DateTime.Now.AddDays(-1);
            userRecord.Salt = string.Empty;

            return true;
        }

        public async Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            User userRecord = await dbContext.Users.FirstOrDefaultAsync(o => o.Id == refreshTokenRequest.UserId && !string.IsNullOrEmpty(o.RefreshToken));

            ValidateRefreshTokenResponse response = new ValidateRefreshTokenResponse();

            if (userRecord == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Invalid session or user already logged out.";
                return response;
            }

            string refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(refreshTokenRequest.RefreshToken, Convert.FromBase64String(userRecord.Salt));

            if (userRecord.RefreshToken != refreshTokenToValidateHash)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Invalid refresh token.";
                return response;
            }
          
            if (userRecord.RefreshTokenExpiration < DateTime.Now)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Refresh token has expired.";
                return response;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.UserId = refreshTokenRequest.UserId;

            return response;
        }

    }
}
