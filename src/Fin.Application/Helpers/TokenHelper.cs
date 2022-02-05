using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fin.Application.Helpers
{
    public class TokenHelper
    {
        public static string GenerateAccessToken(Guid userId, string issuer, string audience, string secret, DateTime expiration)
        {
            byte[] key = Encoding.UTF8.GetBytes(secret);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            });

            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = issuer,
                Audience = audience,
                Expires = expiration,
                SigningCredentials = signingCredentials,

            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public static async Task<string> GenerateRefreshToken()
        {
            byte[] secureRandomBytes = new byte[32];

            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            await Task.Run(() => randomNumberGenerator.GetBytes(secureRandomBytes));

            string refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;
        }
    }
}
