using Idex.Net.Data;
using Idex.Net.Data.Interface;
using System;
using Xunit;

namespace Idex.Net.Tests
{
    public class IdexRepositoryTests : IDisposable
    {
        private IIdexRepository _repo;
        private string _address = "";
        private string _hash = "";

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
            var pair = "ETH_POLY";

            var ticker = _repo.GetTicker(pair).Result;

            Assert.NotNull(ticker);
        }

        [Fact]
        public void GetTickersTest()
        {
            var tickers = _repo.GetTickers().Result;

            Assert.NotNull(tickers);
        }

        [Fact]
        public void GetOrderBookTest()
        {
            var pair = "ETH_POLY";
            var orderBook = _repo.GetOrderBook(pair).Result;

            Assert.NotNull(orderBook);
        }

        [Fact]
        public void GetOrderBookCountTest()
        {
            var pair = "ETH_POLY";
            var count = 10;
            var orderBook = _repo.GetOrderBook(pair, count).Result;

            Assert.NotNull(orderBook);
        }

        [Fact]
        public void GetOpenOrdersTest()
        {
            var pair = "ETH_POLY";
            var openOrders = _repo.GetOpenOrders(pair).Result;

            Assert.NotNull(openOrders);
        }

        [Fact]
        public void GetOpenOrdersCountTest()
        {
            var pair = "ETH_POLY";
            var count = 10;
            var openOrders = _repo.GetOpenOrders(pair, count).Result;

            Assert.NotNull(openOrders);
        }

        [Fact]
        public void GetAddressOpenOrdersTest()
        {
            var openOrders = _repo.GetAddressOpenOrders(_address).Result;

            Assert.NotNull(openOrders);
        }

        [Fact]
        public void GetAddressOpenOrdersCountTest()
        {
            var count = 1;
            var openOrders = _repo.GetAddressOpenOrders(_address, count).Result;

            Assert.NotNull(openOrders);
        }

        [Fact]
        public void GetTradeHistoryTest()
        {
            var pair = "ETH_POLY";
            var trades = _repo.GetTradeHistory(pair).Result;

            Assert.NotNull(trades);
        }

        [Fact]
        public void GetTradeHistoryCountTest()
        {
            var pair = "ETH_POLY";
            var count = 10;
            var trades = _repo.GetTradeHistory(pair, count).Result;

            Assert.NotNull(trades);
        }

        [Fact]
        public void GetTradeHistoryDateRangeTest()
        {
            var pair = "ETH_POLY";
            var start = DateTime.UtcNow.AddDays(-4);
            var end = DateTime.UtcNow.AddDays(-3);
            var trades = _repo.GetTradeHistory(pair, start, end).Result;

            Assert.NotNull(trades);
        }

        [Fact]
        public void GetAddressTradeHistoryTest()
        {
            var trades = _repo.GetAddressTradeHistory(_address).Result;

            Assert.NotNull(trades);
        }

        [Fact]
        public void GetAddressTradeHistoryDateRangeTest()
        {
            var start = DateTime.UtcNow.AddDays(-5);
            var end = DateTime.UtcNow.AddDays(-3);
            var trades = _repo.GetAddressTradeHistory(_address, start, end).Result;

            Assert.NotNull(trades);
        }

        [Fact]
        public void GetCurrenciesTest()
        {
            var currencies = _repo.GetCurrencies().Result;

            Assert.NotNull(currencies);
        }

        [Fact]
        public void GetBalancesTest()
        {
            var balances = _repo.GetBalances(_address).Result;

            Assert.NotNull(balances);
        }

        [Fact]
        public void GetCompleteBalancesTest()
        {
            var balances = _repo.GetCompleteBalances(_address).Result;

            Assert.NotNull(balances);
        }

        [Fact]
        public void GetDepositsTest()
        {
            var deposits = _repo.GetDeposits(_address).Result;

            Assert.NotNull(deposits);
        }

        [Fact]
        public void GetDepositsDateTest()
        {
            var start = DateTime.UtcNow.AddDays(-15);
            var end = DateTime.UtcNow.AddDays(-3);
            var deposits = _repo.GetDeposits(_address,start, end).Result;

            Assert.NotNull(deposits);
        }

        [Fact]
        public void GetWithdrawalsTest()
        {
            var withdrawals = _repo.GetWithdrawals(_address).Result;

            Assert.NotNull(withdrawals);
        }

        [Fact]
        public void GetWithdrawalsDateTest()
        {
            var start = DateTime.UtcNow.AddDays(-45);
            var end = DateTime.UtcNow.AddDays(-3);
            var withdrawals = _repo.GetWithdrawals(_address, start, end).Result;

            Assert.NotNull(withdrawals);
        }

        [Fact]
        public void GetOrderTradesTest()
        {
            var orderTrade = _repo.GetOrderTrades(_hash).Result;

            Assert.NotNull(orderTrade);
        }

        [Fact]
        public void GetContractAddressTest()
        {
            var address = _repo.GetContractAddress().Result;

            Assert.NotNull(address);
        }
    }
}
