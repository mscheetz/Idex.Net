using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class TradeResponse
    {
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public decimal total { get; set; }
        public string market { get; set; }
        public TradeType type { get; set; }
        public decimal price { get; set; }
        public string orderHash { get; set; }
        public string uuid { get; set; }
    }
}
