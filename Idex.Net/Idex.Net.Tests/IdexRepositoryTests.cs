using Idex.Net.Data;
using Idex.Net.Data.Interface;
using System;
using Xunit;

namespace Idex.Net.Tests
{
    public class IdexRepositoryTests : IDisposable
    {
        IIdexRepository _repo;

        public IdexRepositoryTests()
        {
            _repo = new IdexRepository();
        }

        public void Dispose()
        {
        }

        [Fact]
        public void GetTickerTest()
        {
            var pair = "ETH_MANA";

            var ticker = _repo.GetTicker(pair).Result;

            Assert.NotNull(ticker);
        }

        [Fact]
        public void GetTickersTest()
        {
            var tickers = _repo.GetTickers().Result;

            Assert.NotNull(tickers);
        }
    }
}
