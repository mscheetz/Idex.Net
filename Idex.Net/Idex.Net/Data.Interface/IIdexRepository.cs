using Idex.Net.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Idex.Net.Data.Interface
{
    public interface IIdexRepository
    {
        #region Public Endpoints
        /// <summary>
        /// Get ticker for a trading pair
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>Ticker for selected pair</returns>
        Task<Ticker> GetTicker(string pair);

        /// <summary>
        /// Get ticker for all trading pairs
        /// </summary>
        /// <returns>Collection of tickers</returns>
        Task<Dictionary<string, Ticker>> GetTickers();

        /// <summary>
        /// Returns lowest and highest priced open orders for a given market.
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>Collection of orders</returns>
        Task<OrderBook> GetOrderBook(string pair);

        /// <summary>
        /// Returns the best-priced open orders for a given market.
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="count">Number of records to return</param>
        /// <returns>Collection of orders</returns>
        Task<OrderBook> GetOrderBook(string pair, int count);

        /// <summary>
        /// Returns a list of all open orders
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of open orders</returns>
        Task<OpenOrder[]> GetOpenOrders(string pair, int count = 100);

        /// <summary>
        /// Returns a list of all open orders
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of open orders</returns>
        Task<OpenOrder[]> GetAddressOpenOrders(string address, int count = 100);

        /// <summary>
        /// Returns a list of all trades for a given market
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        Task<TradeDetail[]> GetTradeHistory(string pair, int count = 100);

        /// <summary>
        /// Returns a list of all trades for a given market
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        Task<TradeDetail[]> GetTradeHistory(string pair, Sorting sort, int count = 100);

        /// <summary>
        /// Returns a list of all trades for a given market
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        Task<TradeDetail[]> GetTradeHistory(string pair, DateTime startDate, DateTime endDate, int count = 100);

        /// <summary>
        /// Returns a list of all trades for a given market
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of trade detail</returns>
        Task<TradeDetail[]> GetTradeHistory(string pair, DateTime startDate, DateTime endDate, Sorting sort, int count = 100);

        /// <summary>
        /// Returns a list of all trades for a given address. Caution: slow to execute.
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address);

        /// <summary>
        /// Returns a list of all trades for a given address. Caution: slow to execute.
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address, Sorting sort);

        /// <summary>
        /// Returns a list of all trades for a given address. Caution: slow to execute.
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns a list of all trades for a given address. Caution: slow to execute.
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(string address, DateTime startDate, DateTime endDate, Sorting sort);

        /// <summary>
        /// Returns an object of token data indexed by symbol
        /// </summary>
        /// <returns>Collection of currencies and details</returns>
        Task<Dictionary<string, Currency>> GetCurrencies();

        /// <summary>
        /// Returns your available balances 
        /// </summary>
        /// <param name="address">Address to query balances of</param>
        /// <returns>total deposited minus amount in open orders</returns>
        Task<Dictionary<string, decimal>> GetBalances(string address);

        /// <summary>
        /// Returns your available balances
        /// </summary>
        /// <param name="address">Address to query balances of</param>
        /// <returns>Balances and quantity in orders</returns>
        Task<Dictionary<string, Balance>> GetCompleteBalances(string address);

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <returns>Collection of Deposits</returns>
        Task<Deposit[]> GetDeposits(string address);

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Deposits</returns>
        Task<Deposit[]> GetDeposits(string address, DateTime start, DateTime end);

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <returns>Collection of Withdrawals</returns>
        Task<Withdrawal[]> GetWithdrawals(string address);

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <param name="address">Address to query history for</param>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Withdrawals</returns>
        Task<Withdrawal[]> GetWithdrawals(string address, DateTime start, DateTime end);

        /// <summary>
        /// Get all trades involving a given order hash
        /// </summary>
        /// <param name="orderHash">The order hash to query for associated trades</param>
        /// <returns>Trades for a given order hash</returns>
        Task<OrderTrade[]> GetOrderTrades(string orderHash);

        /// <summary>
        /// Returns the contract address used for depositing, withdrawing, and posting orders
        /// </summary>
        /// <returns></returns>
        Task<string> GetContractAddress();

        #endregion Public Endpoints

        #region Authenticated Endpoints

        /// <summary>
        /// Get public address for loaded primary key
        /// </summary>
        /// <returns>String of public address</returns>
        string GetAddress();

        /// <summary>
        /// Returns a list of all open orders
        /// </summary>
        /// <param name="count">Number to be returned (default = 100)</param>
        /// <returns>Collection of open orders</returns>
        Task<OpenOrder[]> GetAddressOpenOrders(int count = 100);

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory();

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(Sorting sort);

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns a list of all trades for a given market or address
        /// </summary>
        /// <param name="start">Start of trade range</param>
        /// <param name="end">End of trade range</param>
        /// <param name="sort">Sorting by transaction date</param>
        /// <returns>Collection of trade detail</returns>
        Task<Dictionary<string, TradeDetail[]>> GetAddressTradeHistory(DateTime startDate, DateTime endDate, Sorting sort);

        /// <summary>
        /// Returns your available balances 
        /// </summary>
        /// <returns>total deposited minus amount in open orders</returns>
        Task<Dictionary<string, decimal>> GetBalances();

        /// <summary>
        /// Returns your available balances
        /// </summary>
        /// <returns>Balances and quantity in orders</returns>
        Task<Dictionary<string, Balance>> GetCompleteBalances();

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <returns>Collection of Deposits</returns>
        Task<Deposit[]> GetDeposits();

        /// <summary>
        /// Returns deposit history
        /// </summary>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Deposits</returns>
        Task<Deposit[]> GetDeposits(DateTime start, DateTime end);

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <returns>Collection of Withdrawals</returns>
        Task<Withdrawal[]> GetWithdrawals();

        /// <summary>
        /// Returns withdrawal history
        /// </summary>
        /// <param name="start">Start of results</param>
        /// <param name="end">End of results</param>
        /// <returns>Collection of Withdrawals</returns>
        Task<Withdrawal[]> GetWithdrawals(DateTime start, DateTime end);

        #endregion Authenticated Endpoints

    }
}
