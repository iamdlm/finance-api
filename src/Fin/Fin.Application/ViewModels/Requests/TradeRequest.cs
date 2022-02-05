using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fin.Application.ViewModels
{
    public class TradeRequest
    {
        [Required]
        public string Date { get; set; }
                
        [Required]
        [JsonPropertyName("number_of_shares")]
        public int NumberOfShares { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public string Currency { get; set; }
                
        [Required]
        [JsonPropertyName("market_value")]
        public decimal MarketValue { get; set; }
        
        [Required]
        public string Action { get; set; }
        
        public string Notes { get; set; }
        
        [Required]
        public string Asset { get; set; }
    }
}
