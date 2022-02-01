using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinApi.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }        
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
