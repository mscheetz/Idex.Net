using Idex.Net.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Idex.Net.Core
{
    public static class Helpers
    {
        /// <summary>
        /// Convert Idex object to POCO
        /// </summary>
        /// <param name="exchangeTickers">Idex Tickers</param>
        /// <returns>Dictionary of local Ticker</returns>
        public static Dictionary<string, Ticker> ExchangeTickerArrToTickerArr(Dictionary<string, ExchangeTicker> exchangeTickers)
        {
            var tickers = new Dictionary<string, Ticker>();

            foreach (var item in exchangeTickers)
            {
                tickers.Add(item.Key, ExhangeTickerToTicker(item.Value));
            }

            return tickers;
        }

        /// <summary>
        /// Convert Idex object to POCO
        /// </summary>
        /// <param name="exTicker">Idex Ticker</param>
        /// <returns>Local Ticker</returns>
        public static Ticker ExhangeTickerToTicker(ExchangeTicker exTicker)
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

        /// <summary>
        /// Get buy symbol from a pair
        /// </summary>
        /// <param name="pair">Pair</param>
        /// <param name="type">Trade type</param>
        /// <returns>String of buy symbol</returns>
        public static string BuySymbol(string pair, TradeType type)
        {
            if (type == TradeType.buy)
                return pair.Substring(pair.IndexOf("_") + 1);
            else
                return pair.Substring(0, pair.IndexOf("_"));
        }

        /// <summary>
        /// Get sell symbol from a pair
        /// </summary>
        /// <param name="pair">Pair</param>
        /// <param name="type">Trade type</param>
        /// <returns>String of sell symbol</returns>
        public static string SellSymbol(string pair, TradeType type)
        {
            if (type == TradeType.buy)
                return pair.Substring(0, pair.IndexOf("_"));
            else
                return pair.Substring(pair.IndexOf("_") + 1);
        }

        /// <summary>
        /// Convert dictionary to querystring
        /// </summary>
        /// <param name="parameters">Dictionary to convert</param>
        /// <returns>String of values</returns>
        public static string StringifyDictionary(Dictionary<string, object> parameters)
        {
            var qsValues = string.Empty;

            if (parameters != null)
            {
                qsValues = string.Join("&", parameters.Select(p => p.Key + "=" + p.Value));
            }

            return qsValues;
        }
    }
}
