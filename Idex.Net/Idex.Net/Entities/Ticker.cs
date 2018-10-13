using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class Ticker
    {
        public decimal last { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal lowestAsk { get; set; }
        public decimal highestBid { get; set; }
        public decimal percentChange { get; set; }
        public decimal baseVolume { get; set; }
        public decimal quoteVolume { get; set; }
    }
}
