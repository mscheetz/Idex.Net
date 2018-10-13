using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class OrderResponse
    {
        public int orderNumber { get; set; }
        public string orderHash { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public decimal total { get; set; }
        public TradeType type { get; set; }
        [JsonProperty(PropertyName = "params")]
        public OrderParams orderParams { get; set; }
    }
}
