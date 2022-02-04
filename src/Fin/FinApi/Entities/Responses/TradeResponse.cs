using System;
using System.Text.Json.Serialization;

namespace FinApi.Entities
{
    public class TradeResponse
    {
        public Guid Id { get; set; }
        
        [JsonPropertyName("user_executor")]
        public Guid UserId { get; set; }
        
        [JsonPropertyName("portfolio")]
        public Guid PortfolioId { get; set; }
        
        public string Date { get; set; }
        
        [JsonPropertyName("number_of_shares")]
        public int NumberOfShares { get; set; }
        
        public decimal Price { get; set; }
        
        public string Currency { get; set; }
        
        [JsonPropertyName("market_value")]
        public decimal MarketValue { get; set; }
        
        public string Action { get; set; }
        
        public string Notes { get; set; }
        
        public string Asset { get; set; }
    }
}
