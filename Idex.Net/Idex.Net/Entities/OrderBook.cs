using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class OrderBook
    {
        public OrderSide[] asks { get; set; }
        public OrderSide[] bids { get; set; }
    }
}
