using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fin.Domain.Enums;

namespace Fin.Domain.Entities
{
    public class Trade : BaseEntity
    {
        public User User { get; set; }
        
        public DateTime Date { get; set; }
        
        public int NumberOfShares { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public string Currency { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal MarketValue { get; set; }
        
        public TradeAction Action { get; set; }
        
        public string Notes { get; set; }
        
        public string Asset { get; set; }
        
        public Portfolio Portfolio { get; set; }
    }
}
