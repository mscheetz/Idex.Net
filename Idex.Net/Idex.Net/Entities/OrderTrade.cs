using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class OrderTrade
    {
        public DateTime date { get; set; }
        public decimal amount { get; set; }
        public TradeType type { get; set; }
        public decimal total { get; set; }
        public decimal price { get; set; }
        public string uuid { get; set; }
        public string transactionHash { get; set; }
    }
}
