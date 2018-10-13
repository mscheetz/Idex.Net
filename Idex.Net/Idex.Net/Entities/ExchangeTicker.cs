using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class ExchangeTicker
    {
        public string last { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string lowestAsk { get; set; }
        public string highestBid { get; set; }
        public string percentChange { get; set; }
        public string baseVolume { get; set; }
        public string quoteVolume { get; set; }
    }
}
