using System.ComponentModel.DataAnnotations;

namespace FinApi.Entities
{
    public class PortfolioRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
