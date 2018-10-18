using FileRepository;
using Idex.Net.Data;
using Idex.Net.Data.Interface;
using System;
using System.Collections.Generic;
using Xunit;

namespace Idex.Net.Tests
{
    public class IdexRepositoryAuthTests : IDisposable
    {
        private IIdexRepository _repo;
        private string _address = "";
        private string _hash = "";
        private string configPath = "config.json";

        public IdexRepositoryAuthTests()
        {
            IFileRepository _fileRepo = new FileRepository.FileRepository();
            Dictionary<string, string> configData = null;
            var publicKey = string.Empty;
            if (_fileRepo.FileExists(configPath))
            {
                configData = _fileRepo.GetDataFromFile<Dictionary<string, string>>(configPath);
            }
            if (configData != null)
            {
                publicKey = configData["privateKey"];
            }
            if (!string.IsNullOrEmpty(publicKey))
            {
                _repo = new IdexRepository(publicKey);
                _address = _repo.GetAddress();
            }
            else
            {
                _repo = new IdexRepository();
            }
        }

        public void Dispose()
        {
        }

        [Fact]
        public void GetAddressTest()
        {
            var address = _repo.GetAddress();

            Assert.NotNull(address);
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
    }
}
