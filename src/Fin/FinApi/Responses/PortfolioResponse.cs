using FinApi.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinApi.Responses
{
    public class PortfolioResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Trade> Trades { get; set; }
    }
}
