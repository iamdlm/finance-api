using System.Text.Json.Serialization;

namespace Fin.Application.ViewModels
{
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("token_expiration")] 
        public string TokenExpiration { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("refresh_token_expiration")]
        public string RefreshTokenExpiration { get; set; }

    }
}
