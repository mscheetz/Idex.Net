using DateTimeHelpers;
using Idex.Net.Data.Interface;
using Idex.Net.Entities;
//using RESTApiAccess;
//using RESTApiAccess.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Idex.Net.Data
{
    public class IdexRepository : IIdexRepository
    {
        private string baseUrl;
        private IRESTRepository _restRepo;
        private DateTimeHelper _dtHelper;

        public IdexRepository()
        {
            LoadRepository();
        }

        private void LoadRepository()
        {
            _restRepo = new RESTRepository();
            baseUrl = "https://api.idex.market";
            _dtHelper = new DateTimeHelper();
        }

        public async Task<Ticker> GetTicker(string pair)
        {
            string url = baseUrl + "/returnTicker";

            var parameters = new Dictionary<string, object>();
            parameters.Add("market", pair);

            var response = await _restRepo.PostApi<ExchangeTicker, Dictionary<string, object>>(url, parameters);

            return ExhangeTickerToTicker(response);
        }

        public async Task<Dictionary<string, Ticker>> GetTickers()
        {
            string url = baseUrl + "/returnTicker";

            var response = await _restRepo.PostApi<Dictionary<string, ExchangeTicker>>(url);

            return ExchangeTickerArrToTickerArr(response);
        }

        public async Task<OrderBook> GetOrderBook(string pair)
        {
            return await OnGetOrderBook(pair, null);
        }

        public async Task<OrderBook> GetOrderBook(string pair, int count)
        {
            return await OnGetOrderBook(pair, count);
        }

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

        public async Task<OpenOrder[]> GetOpenOrders(string pair)
        {
            return await OnGetOpenOrders(pair, string.Empty, null, string.Empty);
        }

        public async Task<OpenOrder[]> GetOpenOrders(string pair, int count)
        {
            return await OnGetOpenOrders(pair, string.Empty, count, string.Empty);
        }

        public async Task<OpenOrder[]> GetAddressOpenOrders(string address)
        {
            return await OnGetOpenOrders(string.Empty, address, null, string.Empty);
        }

        public async Task<OpenOrder[]> GetAddressOpenOrdersFor(string address, int count)
        {
            return await OnGetOpenOrders(string.Empty, address, count, string.Empty);
        }

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

        public async Task<TradeDetail[]> GetTradeHistory(string pair)
        {
            return await OnGetTradeHistory(pair, string.Empty, null, null, null, null, string.Empty);
        }

        public async Task<TradeDetail[]> GetTradeHistory(string pair, Sorting sort)
        {
            return await OnGetTradeHistory(pair, string.Empty, null, null, sort, null, string.Empty);
        }

        public async Task<TradeDetail[]> GetTradeHistory(string pair, DateTime startDate, DateTime endDate)
        {
            if(startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetTradeHistory(pair, string.Empty, start, end, null, null, string.Empty);
        }

        public async Task<TradeDetail[]> GetTradeHistory(string pair, DateTime startDate, DateTime endDate, Sorting sort)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetTradeHistory(pair, string.Empty, start, end, sort, null, string.Empty);
        }

        public async Task<TradeDetail[]> GetAddressTradeHistory(string address)
        {
            return await OnGetTradeHistory(string.Empty, address, null, null, null, null, string.Empty);
        }

        public async Task<TradeDetail[]> GetAddressTradeHistory(string address, Sorting sort)
        {
            return await OnGetTradeHistory(string.Empty, address, null, null, sort, null, string.Empty);
        }

        public async Task<TradeDetail[]> GetAddressTradeHistory(string address, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetTradeHistory(string.Empty, address, start, end, null, null, string.Empty);
        }

        public async Task<TradeDetail[]> GetAddressTradeHistory(string address, DateTime startDate, DateTime endDate, Sorting sort)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start Date cannot be after End Date");
            }

            var start = _dtHelper.UTCtoUnixTime(startDate);
            var end = _dtHelper.UTCtoUnixTime(endDate);
            return await OnGetTradeHistory(string.Empty, address, start, end, sort, null, string.Empty);
        }

        private async Task<TradeDetail[]> OnGetTradeHistory(string pair, string address, long? start, long? end, Sorting? sort, int? count, string cursor)
        {
            if (string.IsNullOrEmpty(pair) && string.IsNullOrEmpty(address))
            {
                throw new Exception("A trading pair or ETH address must be identified");
            }

            string url = baseUrl + "/returnTradeHistory";

            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(pair))
                parameters.Add("market", pair);
            if (!string.IsNullOrEmpty(address))
                parameters.Add("address", address);
            if (start != 0)
                parameters.Add("start", start);
            if (end != 0)
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

        public async Task<Dictionary<string, Currency>> GetCurrencies()
        {
            string url = baseUrl + "/returnCurrencies";

            var response = await _restRepo.PostApi<Dictionary<string, Currency>>(url);

            return response;
        }

        public async Task<Dictionary<string, Balance>> GetBalances(string address)
        {
            string url = baseUrl + "/returnCurrencies";

            var parameters = new Dictionary<string, object>();
            
            parameters.Add("address", address);

            var response = await _restRepo.PostApi<Dictionary<string, Balance>, Dictionary<string, object>>(url, parameters);

            return response;
        }

        public async Task<OrderResponse> PlaceOrder(string pair, decimal price, decimal quantity)
        {
            string url = baseUrl + "/returnCurrencies";

            var parameters = new Dictionary<string, object>();

            parameters.Add("tokenBuy", string.Empty);
            parameters.Add("amountBuy", 0.0M);
            parameters.Add("tokenSell", string.Empty);
            parameters.Add("amountSell", 0.0M);
            parameters.Add("address", string.Empty);
            parameters.Add("nonce", 0);
            parameters.Add("expires", 0);

            //SignMessage

            parameters.Add("v",0);
            parameters.Add("r", string.Empty);
            parameters.Add("s", string.Empty);


            var response = await _restRepo.PostApi<OrderResponse, Dictionary<string, object>>(url, parameters);

            return response;
        }

        private Dictionary<string, Ticker> ExchangeTickerArrToTickerArr(Dictionary<string, ExchangeTicker> exchangeTickers)
        {
            var tickers = new Dictionary<string, Ticker>();

            foreach(var item in exchangeTickers)
            {
                tickers.Add(item.Key, ExhangeTickerToTicker(item.Value));
            }

            return tickers;
        }

        private Ticker ExhangeTickerToTicker(ExchangeTicker exTicker)
        {
            var ticker = new Ticker
            {
                baseVolume = !exTicker.baseVolume.Equals("N/A") ? decimal.Parse(exTicker.baseVolume) : 0.0M,
                high = !exTicker.high.Equals("N/A") ? decimal.Parse(exTicker.high) : 0.0M,
                highestBid = !exTicker.highestBid.Equals("N/A") ? decimal.Parse(exTicker.highestBid) : 0.0M,
                last = !exTicker.last.Equals("N/A") ? decimal.Parse(exTicker.last) : 0.0M,
                low = !exTicker.low.Equals("N/A") ? decimal.Parse(exTicker.low) : 0.0M,
                lowestAsk = !exTicker.lowestAsk.Equals("N/A") ? decimal.Parse(exTicker.lowestAsk) : 0.0M,
                percentChange = !exTicker.percentChange.Equals("N/A") ? decimal.Parse(exTicker.percentChange) : 0.0M,
                quoteVolume = !exTicker.quoteVolume.Equals("N/A") ? decimal.Parse(exTicker.quoteVolume) : 0.0M,
            };

            return ticker;
        }
    }
}
