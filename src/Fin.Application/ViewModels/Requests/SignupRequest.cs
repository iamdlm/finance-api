using System.ComponentModel.DataAnnotations;

namespace Fin.Application.ViewModels
{
    public class SignupRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Name { get; set; }

    }
}
