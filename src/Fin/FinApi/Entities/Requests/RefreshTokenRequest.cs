using System.ComponentModel.DataAnnotations;

namespace FinApi.Entities
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
