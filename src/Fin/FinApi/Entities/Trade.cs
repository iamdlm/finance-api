using System;

namespace FinApi.Entities
{
    public class Trade : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserExecutor { get; set; }
        public string Date { get; set; }
        public int NumberOfShares { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public float MarketValue { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public string Asset { get; set; }
        public Portfolio Portfolio { get; set; }
    }
}
