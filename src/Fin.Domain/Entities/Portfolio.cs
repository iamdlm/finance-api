using System.Collections.Generic;

namespace Fin.Domain.Entities
{
    public class Portfolio : BaseEntity
    {
        public string Name { get; set; }
        public User User { get; set; }
        public ICollection<Trade> Trades { get; set; }
    }
}
