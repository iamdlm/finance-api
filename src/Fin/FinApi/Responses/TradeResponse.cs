using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinApi.Responses
{
    public class TradeResponse
    {
        public Guid Id { get; set; }
        [JsonPropertyName("user_executor")]
        public string UserExecutor { get; set; }
        public Guid Portfolio { get; set; }
        public string Date { get; set; }
        [JsonPropertyName("number_of_shares")]
        public int NumberOfShares { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        [JsonPropertyName("market_value")]
        public float MarketValue { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public string Asset { get; set; }
    }
}
