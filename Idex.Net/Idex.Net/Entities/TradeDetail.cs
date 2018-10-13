using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class TradeDetail
    {
        public DateTime date { get; set; }
        public decimal amount { get; set; }
        public TradeType type { get; set; }
        public decimal total { get; set; }
        public decimal price { get; set; }
        public string orderHash { get; set; }
        public string uuid { get; set; }
        public long tid { get; set; }
        public decimal buyerFee { get; set; }
        public decimal sellerFee { get; set; }
        public decimal gasFee { get; set; }
        public long timestamp { get; set; }
        public string maker { get; set; }
        public string taker { get; set; }
        public string transactionHash { get; set; }
        public decimal usdValue { get; set; }
    }
}
