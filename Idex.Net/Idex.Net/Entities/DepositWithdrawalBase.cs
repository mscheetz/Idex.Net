using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class DepositWithdrawalBase
    {
        public string currency { get; set; }
        public decimal amount { get; set; }
        public long timestamp { get; set; }
        public string transactionHash { get; set; }
    }
}
