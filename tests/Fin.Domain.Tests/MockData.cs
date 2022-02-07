using Fin.Domain.Entities;
using Fin.Domain.Enums;
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
                    PortfolioB2,
                    PortfolioB3,
                    PortfolioB4
                };
            }
        }

        public static List<Trade> Trades
        {
            get
            {
                return new List<Trade>
                {
                    TradePB2,
                    TradePB3N1,
                    TradePB3N2,
                    TradePB3N3
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

        public static readonly Portfolio PortfolioB3 = new Portfolio
        {
            Id = new Guid("ce8174d2-2522-4b7a-9538-bd1984c5cf49"),
            Name = "PB3",
            User = UserB
        };

        public static readonly Portfolio PortfolioB4 = new Portfolio
        {
            Id = new Guid("3edff360-0f04-43d7-aa4d-ba11510276c7"),
            Name = "PB4",
            User = UserB
        };

        public static readonly Trade TradePB2 = new Trade
        {
            Id = new Guid("d201c7e5-8858-41d6-a20f-2e338cfb6164"),
            Date = new DateTime(),
            Action = TradeAction.Buy,
            Asset = "BTC",
            Currency = "EUR",
            MarketValue = 70000,
            NumberOfShares = 2,
            Price = 35000,
            Notes = string.Empty,
            User = UserB,
            Portfolio = PortfolioB2
        };

        public static readonly Trade TradePB3N1 = new Trade
        {
            Id = new Guid("d201c7e5-8858-41d6-a20f-2e338cfb0001"),
            Date = new DateTime(),
            Action = TradeAction.Buy,
            Asset = "ETH",
            Currency = "EUR",
            MarketValue = 25000,
            NumberOfShares = 10,
            Price = 2500,
            Notes = string.Empty,
            User = UserB,
            Portfolio = PortfolioB3
        };

        public static readonly Trade TradePB3N2 = new Trade
        {
            Id = new Guid("d201c7e5-8858-41d6-a20f-2e338cfb0002"),
            Date = new DateTime(),
            Action = TradeAction.Sell,
            Asset = "ETH",
            Currency = "EUR",
            MarketValue = 17500,
            NumberOfShares = 5,
            Price = 3500,
            Notes = string.Empty,
            User = UserB,
            Portfolio = PortfolioB3
        };

        public static readonly Trade TradePB3N3 = new Trade
        {
            Id = new Guid("d201c7e5-8858-41d6-a20f-2e338cfb0003"),
            Date = new DateTime(),
            Action = TradeAction.Buy,
            Asset = "CASH",
            Currency = "EUR",
            MarketValue = 1000,
            NumberOfShares = 1,
            Price = 1000,
            Notes = string.Empty,
            User = UserB,
            Portfolio = PortfolioB3
        };
    }
}
