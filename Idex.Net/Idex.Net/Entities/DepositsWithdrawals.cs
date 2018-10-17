using System;
using System.Collections.Generic;
using System.Text;

namespace Idex.Net.Entities
{
    public class DepositsWithdrawals
    {
        public Deposit[] deposits { get; set; }
        public Withdrawal[] withdrawals { get; set; }
    }
}
