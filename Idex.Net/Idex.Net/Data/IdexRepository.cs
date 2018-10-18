using DateTimeHelpers;
using Idex.Net.Core;
using Idex.Net.Data.Interface;
using Idex.Net.Entities;
using Nethereum.Signer;
//using RESTApiAccess;
//using RESTApiAccess.Interface;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Idex.Net.Data
{
    public class IdexRepository : IIdexRepository
    {
        private string baseUrl;
        private IRESTRepository _restRepo;
        private DateTimeHelper _dtHelper;
        private SecureString _privateKey = null;
        private Dictionary<string, Currency> currencyList;
        private string _address = string.Empty;
        private string _contractAddress = string.Empty;

        public IdexRepository()
        {
            LoadRepository();
        }

        public IdexRepository(string privateKey)
        {
            this._privateKey = Security.ToSecureString(privateKey);

            LoadRepository();
        }

        private void LoadRepository()
        {
            _restRepo = new RESTRepository();
            baseUrl = "https://api.idex.market";
            _dtHelper = new DateTimeHelper();
            if(_privateKey != null)
            {
                _address = EthECKey.GetPublicAddress(Security.ToUnsecureString(_privateKey));
                LazyLoadElements();
            }
        }

        private async void LazyLoadElements()
        {
            currencyList = await this.GetCurrencies();
            _contractAddress = await GetContractAddress();
        }

        #region Public Endpoints

        /// <summary>
        /// Get ticker for a trading pair
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>Ticker for selected pair</returns>
        public async Task<Ticker> GetTicker(string pair)
        {
            string url = baseUrl + "/returnTicker";

            var parameters = new Dictionary<string, object>();
            parameters.Add("market", pair);

            var response = await _restRepo.PostApi<ExchangeTicker, Dictionary<string, object>>(url, parameters);

            return Helpers.ExhangeTickerToTicker(response);
        }

        /// <summary>
        /// Get ticker for all trading pairs
        /// </summary>
        /// <returns>Collection of tickers</returns>
        public async Task<Dictionary<string, Ticker>> GetTickers()
        {
            string url = baseUrl + "/returnTicker";

            var response = await _restRepo.PostApi<Dictionary<string, ExchangeTicker>>(url);

            return Helpers.ExchangeTickerArrToTickerArr(response);
        }

        /// <summary>
        /// Returns lowest and highest priced open orders for a given market.
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>Collection of orders</returns>
        public async Task<OrderBook> GetOrderBook(string pair)
        {
            return await OnGetOrderBook(pair, null);
        }

        /// <summary>
        /// Returns the best-priced open orders for a given market.
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="count">Number of records to return</param>
        /// <returns>Collection of orders</returns>
        public async Task<OrderBook> GetOrderBook(string pair, int count)
        {
            return await OnGetOrderBook(pair, count);
        }

        /// <summary>
        /// Returns the best-priced open orders for a given market.
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="count">Number of records to return</param>
        /// <returns>Collection of orders</returns>
        private async Task<OrderBook> OnGetOrderBook(string pair, int? count)
        {
            string url = baseUrl + "/returnOrderBook";

            var parameters = new Dictionary<string, object>();
            parameters.Add("market", pair);
            if (count != null)
            {
                count = count > 100 ? 100 : count;
                count = count < 1 ? 1 : count;
                parameters.Add("count", (int)count);
            }

            var response = await _restRepo.PostApi<OrderBook, Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns a list of all open orders
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of open orders</returns>
        public async Task<OpenOrder[]> GetOpenOrders(string pair, int count = 100)
        {
            return await OnGetOpenOrders(pair, string.Empty, count, string.Empty);
        }

        /// <summary>
        /// Returns a list of all open orders
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of open orders</returns>
        public async Task<OpenOrder[]> GetAddressOpenOrders(string address, int count = 100)
        {
            return await OnGetOpenOrders(string.Empty, address, count, string.Empty);
        }

        /// <summary>
        /// Returns a list of all open orders
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="address">Address to query</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <param name="cursor">Page number to return</param>
        /// <returns>Collection of open orders</returns>
        private async Task<OpenOrder[]> OnGetOpenOrders(string pair, string address, int? count, string cursor)
        {
            if(string.IsNullOrEmpty(pair) && string.IsNullOrEmpty(address))
            {
                throw new Exception("A trading pair or ETH address must be identified");
            }

            string url = baseUrl + "/returnOpenOrders";

            var parameters = new Dictionary<string, object>();
            if(!string.IsNullOrEmpty(pair))
                parameters.Add("market", pair);
            if (!string.IsNullOrEmpty(address))
                parameters.Add("address", address);

            if (count != null)
            {
                count = count > 100 ? 100 : count;
                count = count < 1 ? 1 : count;
                parameters.Add("count", (int)count);
            }
            if (!string.IsNullOrEmpty(cursor))
                parameters.Add("cursor", cursor);

            var response = await _restRepo.PostApi<OpenOrder[], Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<TradeDetail[]> GetTradeHistory(string pair, int count = 100)
        {
            return await OnGetTradeHistory(pair, null, null, null, count, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<TradeDetail[]> GetTradeHistory(string pair, Sorting sort, int count = 100)
        {
            return await OnGetTradeHistory(pair, null, null, sort, count, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<TradeDetail[]> GetTradeHistory(string pair, DateTime startDate, DateTime endDate, int count = 100)
        {
            if(startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetTradeHistory(pair, start, end, null, count, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<TradeDetail[]> GetTradeHistory(string pair, DateTime startDate, DateTime endDate, Sorting sort, int count = 100)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetTradeHistory(pair, start, end, sort, count, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address)
        {
            return await OnGetAddressTradeHistory(address, null, null, null, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address, Sorting sort)
        {
            return await OnGetAddressTradeHistory(address, null, null, sort, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetAddressTradeHistory(address, start, end, null, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address, DateTime startDate, DateTime endDate, Sorting sort)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetAddressTradeHistory(address, start, end, sort, string.Empty);
        }

        /// <summary>
        /// Returns a paginated list of all trades for a given market
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <param name="count">Number to be returned</param>
        /// <param name="cursor">Page number to return</param>
        /// <returns>Collection of trade detail</returns>
        private async Task<TradeDetail[]> OnGetTradeHistory(string pair, long? start, long? end, Sorting? sort, int? count, string cursor)
        {
            string url = baseUrl + "/returnTradeHistory";

            var parameters = new Dictionary<string, object>();
            parameters.Add("market", pair);

            if (start != null && start != 0)
                parameters.Add("start", start);
            if (end != null && end != 0)
                parameters.Add("end", end);
            if (sort != null)
                parameters.Add("sort", sort.ToString());
            if (count != null)
            {
                count = count > 100 ? 100 : count;
                count = count < 1 ? 1 : count;
                parameters.Add("count", (int)count);
            }
            if (!string.IsNullOrEmpty(cursor))
                parameters.Add("cursor", cursor);

            var response = await _restRepo.PostApi<TradeDetail[], Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns a paginated list of all trades for a given address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <param name="cursor">Page number to return</param>
        /// <returns>Collection of trade detail</returns>
        private async Task<Dictionary<string,TradeDetail[]>> OnGetAddressTradeHistory(string address, long? start, long? end, Sorting? sort, string cursor)
        {

            string url = baseUrl + "/returnTradeHistory";

            var parameters = new Dictionary<string, object>();
            parameters.Add("address", address);

            if (start != null && start != 0)
                parameters.Add("start", start);
            if (end != null && end != 0)
                parameters.Add("end", end);
            if (sort != null)
                parameters.Add("sort", sort.ToString());
            if (!string.IsNullOrEmpty(cursor))
                parameters.Add("cursor", cursor);

            var response = await _restRepo.PostApi<Dictionary<string, TradeDetail[]>, Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns an object of token data indexed by symbol
        /// </summary>
        /// <returns>Collection of currencies and details</returns>
        public async Task<Dictionary<string, Currency>> GetCurrencies()
        {
            string url = baseUrl + "/returnCurrencies";

            var response = await _restRepo.PostApi<Dictionary<string, Currency>>(url);

            return response;
        }

        /// <summary>
        /// Returns your available balances 
        /// </summary>
        /// <param name="address">Address to query balances of</param>
        /// <returns>total deposited minus amount in open orders</returns>
        public async Task<Dictionary<string, decimal>> GetBalances(string address)
        {
            string url = baseUrl + "/returnBalances";

            var parameters = new Dictionary<string, object>();
            
            parameters.Add("address", address);

            var response = await _restRepo.PostApi<Dictionary<string, decimal>, Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns your available balances
        /// </summary>
        /// <param name="address">Address to query balances of</param>
        /// <returns>Balances and quantity in orders</returns>
        public async Task<Dictionary<string, Balance>> GetCompleteBalances(string address)
        {
            string url = baseUrl + "/returnCompleteBalances";

            var parameters = new Dictionary<string, object>();

            parameters.Add("address", address);

            var response = await _restRepo.PostApi<Dictionary<string, Balance>, Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <returns>Collection of Deposits</returns>
        public async Task<Deposit[]> GetDeposits(string address)
        {
            var depositWithdrawals = await OnGetDepositsWithdrawals(address, null, null);

            return depositWithdrawals.deposits;
        }

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Deposits</returns>
        public async Task<Deposit[]> GetDeposits(string address, DateTime start, DateTime end)
        {
            var depositWithdrawals = await OnGetDepositsWithdrawals(address, start, end);

            return depositWithdrawals.deposits;
        }

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <returns>Collection of Withdrawals</returns>
        public async Task<Withdrawal[]> GetWithdrawals(string address)
        {
            var depositWithdrawals = await OnGetDepositsWithdrawals(address, null, null);

            return depositWithdrawals.withdrawals;
        }

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Withdrawals</returns>
        public async Task<Withdrawal[]> GetWithdrawals(string address, DateTime start, DateTime end)
        {
            var depositWithdrawals = await OnGetDepositsWithdrawals(address, start, end);

            return depositWithdrawals.withdrawals;
        }

        /// <summary>
        /// Returns deposit and withdrawal history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Deposits and Withdrawals</returns>
        private async Task<DepositsWithdrawals> OnGetDepositsWithdrawals(string address, DateTime? start, DateTime? end)
        {
            if(start !=null && end !=null && start > end)
            {
                throw new Exception("Start time must be before end time!");
            }

            string url = baseUrl + "/returnDepositsWithdrawals";

            var parameters = new Dictionary<string, object>();

            parameters.Add("address", address);
            if (start != null)
                parameters.Add("start", _dtHelper.UTCtoUnixTime((DateTime)start));
            if (end != null)
                parameters.Add("end", _dtHelper.UTCtoUnixTime((DateTime)end));

            var response = await _restRepo.PostApi<DepositsWithdrawals, Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Get all trades involving a given order hash
        /// </summary>
        /// <param name="orderHash">The order hash to query for associated trades</param>
        /// <returns>Trades for a given order hash</returns>
        public async Task<OrderTrade[]> GetOrderTrades(string orderHash)
        {
            string url = baseUrl + "/returnOrderTrades";

            var parameters = new Dictionary<string, object>();

            parameters.Add("orderHash", orderHash);

            var response = await _restRepo.PostApi<OrderTrade[], Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns the contract address used for depositing, withdrawing, and posting orders
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetContractAddress()
        {
            string url = baseUrl + "/returnContractAddress";
            
            var response = await _restRepo.PostApi<Dictionary<string, string>>(url);

            return response["address"];
        }

        #endregion Public Endpoints

        #region Authenticated Endpoints

        /// <summary>
        /// Get public address for loaded primary key
        /// </summary>
        /// <returns>String of public address</returns>
        public string GetAddress()
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            return _address;
        }

        /// <summary>
        /// Returns a list of all open orders
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of open orders</returns>
        public async Task<OpenOrder[]> GetAddressOpenOrders(int count = 100)
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            return await OnGetOpenOrders(string.Empty, _address, count, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory()
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            return await OnGetAddressTradeHistory(_address, null, null, null, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(Sorting sort)
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            return await OnGetAddressTradeHistory(_address, null, null, sort, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            if (startDate > endDate)
                throw new Exception("Start Date cannot be after End Date");

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetAddressTradeHistory(_address, start, end, null, string.Empty);
        }

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        public async Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(DateTime startDate, DateTime endDate, Sorting sort)
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            if (startDate > endDate)
                throw new Exception("Start Date cannot be after End Date");

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetAddressTradeHistory(_address, start, end, sort, string.Empty);
        }

        /// <summary>
        /// Returns your available balances 
        /// </summary>
        /// <param name="address">Address to query balances of</param>
        /// <returns>total deposited minus amount in open orders</returns>
        public async Task<Dictionary<string, decimal>> GetBalances()
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            string url = baseUrl + "/returnBalances";

            var parameters = new Dictionary<string, object>();

            parameters.Add("address", _address);

            var response = await _restRepo.PostApi<Dictionary<string, decimal>, Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns your available balances
        /// </summary>
        /// <param name="address">Address to query balances of</param>
        /// <returns>Balances and quantity in orders</returns>
        public async Task<Dictionary<string, Balance>> GetCompleteBalances()
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            string url = baseUrl + "/returnCompleteBalances";

            var parameters = new Dictionary<string, object>();

            parameters.Add("address", _address);

            var response = await _restRepo.PostApi<Dictionary<string, Balance>, Dictionary<string, object>>(url, parameters);

            return response;
        }

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <returns>Collection of Deposits</returns>
        public async Task<Deposit[]> GetDeposits()
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            var depositWithdrawals = await OnGetDepositsWithdrawals(_address, null, null);

            return depositWithdrawals.deposits;
        }

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Deposits</returns>
        public async Task<Deposit[]> GetDeposits(DateTime start, DateTime end)
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            var depositWithdrawals = await OnGetDepositsWithdrawals(_address, start, end);

            return depositWithdrawals.deposits;
        }

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <returns>Collection of Withdrawals</returns>
        public async Task<Withdrawal[]> GetWithdrawals()
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            var depositWithdrawals = await OnGetDepositsWithdrawals(_address, null, null);

            return depositWithdrawals.withdrawals;
        }

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Withdrawals</returns>
        public async Task<Withdrawal[]> GetWithdrawals(DateTime start, DateTime end)
        {
            if (string.IsNullOrEmpty(_address))
                throw new Exception("Load Idex.Net with wallet Primary Key to use this endpoint.");

            var depositWithdrawals = await OnGetDepositsWithdrawals(_address, start, end);

            return depositWithdrawals.withdrawals;
        }

        /// <summary>
        /// Place an order on the exchange
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Price of trade</param>
        /// <param name="quantity">Quantity to trade</param>
        /// <param name="type">Trade side ( buy | sell )</param>
        /// <returns></returns>
        public async Task<OrderResponse> PlaceOrder(string pair, decimal price, decimal quantity, TradeType type)
        {
            string url = baseUrl + "/returnCurrencies";

            var buySymbol = Helpers.BuySymbol(pair, type);
            var sellSymbol = Helpers.SellSymbol(pair, type);
            BigInteger buyAmount = type == TradeType.buy ? new BigInteger(quantity) : new BigInteger((price * quantity));
            BigInteger sellAmount = type == TradeType.buy ? new BigInteger((price * quantity)) : new BigInteger(quantity);

            var parameters = new Dictionary<string, object>();
            parameters.Add("contractAddress", _contractAddress);
            parameters.Add("tokenBuy", currencyList[buySymbol].address);
            parameters.Add("amountBuy", buyAmount);
            parameters.Add("tokenSell", currencyList[sellSymbol].address);
            parameters.Add("amountSell", sellAmount);
            parameters.Add("nonce", new BigInteger(_dtHelper.UTCtoUnixTime()));
            parameters.Add("address", _address);

            //SignMessage
            var signature = new Nethereum.Signer.TransactionSigner();
            

            parameters.Add("v",0);
            parameters.Add("r", string.Empty);
            parameters.Add("s", string.Empty);


            var response = await _restRepo.PostApi<OrderResponse, Dictionary<string, object>>(url, parameters);

            return response;
        }

        #endregion Authenticated Endpoints
    }
}
