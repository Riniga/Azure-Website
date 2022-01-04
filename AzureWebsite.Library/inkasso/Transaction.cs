using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AzureWebsite.Library.Inkasso
{
    public class Transaction
    {
        public int Id { get; set; }
        public Debt Debt{ get; set; }
        [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
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
