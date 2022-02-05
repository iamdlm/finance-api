using System.ComponentModel.DataAnnotations;

namespace Fin.Application.ViewModels
{
    public class PortfolioRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
