using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class OpenOrder
    {
        public long timestamp { get; set; }
        public string orderHash { get; set; }
        public string market { get; set; }
        public TradeType type { get; set; }
        public long orderNumber { get; set; }
        [JsonProperty(PropertyName = "params")]
        public OrderParams orderParams { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public decimal total { get; set; }
    }
}
