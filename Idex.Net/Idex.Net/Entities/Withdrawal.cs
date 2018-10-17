using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class Withdrawal : DepositWithdrawalBase
    {
        public int withdrawalNumber { get; set; }
        public string status { get; set; }
    }
}
