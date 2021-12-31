using System;
using System.Collections.Generic;

namespace AzureWebsite.Library.Inkasso
{
    public class Transaction
    {
        public int Id { get; set; }
        public Debt Debt{ get; set; }
        public TransactionType TransactionType{ get; set; }
        public DateTime TimeStamp{ get; set; }
        public decimal Amount { get; set; }
    }

    public enum TransactionType
    { 
        SetBalance,
        Invoice,
        Payment,
        Interest
    }


}
