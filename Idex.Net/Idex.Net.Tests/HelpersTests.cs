using Idex.Net.Core;
using Idex.Net.Data;
using Idex.Net.Data.Interface;
using Idex.Net.Entities;
using System;
using Xunit;

namespace Idex.Net.Tests
{
    public class HelpersTests : IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public void GetBuySymbolBuyTest()
        {
            var pair = "ETH_POLY";
            var type = TradeType.buy;

            var symbol = Helpers.BuySymbol(pair, type);

            Assert.Equal("POLY", symbol);
        }

        [Fact]
        public void GetBuySymbolSellTest()
        {
            var pair = "ETH_POLY";
            var type = TradeType.sell;

            var symbol = Helpers.BuySymbol(pair, type);

            Assert.Equal("ETH", symbol);
        }

        [Fact]
        public void GetSellSymbolBuyTest()
        {
            var pair = "ETH_POLY";
            var type = TradeType.buy;

            var symbol = Helpers.SellSymbol(pair, type);

            Assert.Equal("ETH", symbol);
        }

        [Fact]
        public void GetSellSymbolSellTest()
        {
            var pair = "ETH_POLY";
            var type = TradeType.sell;

            var symbol = Helpers.SellSymbol(pair, type);

            Assert.Equal("POLY", symbol);
        }
    }
}
