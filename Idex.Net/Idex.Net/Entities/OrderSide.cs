using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class OrderSide
    {
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public decimal total { get; set; }
        public string orderHash { get; set; }
        [JsonProperty(PropertyName = "params")]
        public OrderParams orderParams { get; set; }
}
}
