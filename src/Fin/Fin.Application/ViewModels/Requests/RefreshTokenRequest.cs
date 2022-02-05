using System.ComponentModel.DataAnnotations;

namespace Fin.Application.ViewModels
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
