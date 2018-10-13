using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class OrderParams
    {
        public string tokenBuy { get; set; }
        public string buySymbol { get; set; }
        public int buyPrecision { get; set; }
        public decimal amountBuy { get; set; }
        public string tokenSell { get; set; }
        public string sellSymbol { get; set; }
        public int sellPrecision { get; set; }
        public decimal sellAmount { get; set; }
        public long expires { get; set; }
        public long nonce { get; set; }
        public string user { get; set; }
    }
}
