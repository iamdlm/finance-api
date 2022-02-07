using Fin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fin.Domain.Tests
{
    public class MockData
    {


        public static List<User> Users
        {
            get
            {
                return new List<User>
                {
                    UserA,
                    UserB
                };
            }
        }

        public static List<Portfolio> Portfolios
        {
            get
            {
                return new List<Portfolio>
                {
                    PortfolioB1,
                    PortfolioB2
                };
            }
        }

        public static List<Trade> Trades
        {
            get
            {
                return new List<Trade>
                {
                    TradePB2
                };
            }
        }

        public static readonly User UserA = new User
        {
            Id = new Guid("7ab39994-e375-450d-851d-3dc92a7e1fad"),
            Name = "AAA",
            Username = "aaa",
            Email = "aaa@example.com"
        };

        public static readonly User UserB = new User
        {
            Id = new Guid("50dbc4f6-eb9b-4231-8d99-6bc0c840ac47"),
            Name = "BBB",
            Username = "bbb",
            Email = "bbb@example.com"
        };

        public static readonly Portfolio PortfolioB1 = new Portfolio
        {
            Id = new Guid("a04f5e35-5bc3-480f-8ae8-a3b687940b87"),
            Name = "PB1",
            User = UserB
        };

        public static readonly Portfolio PortfolioB2 = new Portfolio
        {
            Id = new Guid("0c838e26-5f8e-4c20-8aec-1d20427711fc"),
            Name = "PB2",
            User = UserB
        };

        public static readonly Trade TradePB2 = new Trade
        {
            Id = new Guid("d201c7e5-8858-41d6-a20f-2e338cfb6164"),
            Date = new DateTime(),
            Action = "buy",
            Asset = "BTC",
            Currency = "EUR",
            MarketValue = 1000,
            NumberOfShares = 10,
            Price = 100,
            Notes = string.Empty,
            User = UserB,
            Portfolio = PortfolioB2
        };
    }
}
