﻿using System;
using System.Collections.Generic;

namespace AzureWebsite.Library.Inkasso
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid DebtId { get; set; }
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