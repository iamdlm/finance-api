using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fin.Application.Services
{
    public static class AssetService
    {
        private static readonly Dictionary<string, decimal> Assets = new Dictionary<string, decimal>()
        {
            { "BTC", 45000.00m },
            { "ETH", 3000.00m },
            { "ADA", 1.00m },
            { "SOL", 120.00m }
        };

        private static readonly Random Random = new Random();

        public static async Task<decimal> GetAssetCurrentPriceAsync(string assetName)
        {
            bool assetExists = Assets.ContainsKey(assetName);
            if (assetExists)
            {
                return await Task.FromResult(Assets[assetName]);
            }
            else
            {
                decimal value = Random.Next(1, 50);
                Assets.Add(assetName, value);
                return await Task.FromResult(value);
            }
        }
    }
}
