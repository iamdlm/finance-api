using System;
using System.Collections.Generic;

namespace FinApi.Entities
{
    public class Portfolio : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public ICollection<Trade> Trades { get; set; }
    }
}
