using System;

namespace Fin.Application.DTOs
{
    public class RefreshTokenDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
