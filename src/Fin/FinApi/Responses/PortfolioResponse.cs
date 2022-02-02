using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinApi.Responses
{
    public class PortfolioResponse : BaseResponse
    {
        public string Name { get; set; }
        public ICollection<TradeResponse> Trades { get; set; }
    }
}
