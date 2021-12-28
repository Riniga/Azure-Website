using System;
using System.Collections.Generic;

namespace AzureWebsite.Library.Inkasso
{
    public class Debt
    {
        public Guid Id { get; set; }
        public Person Person{ get; set; }
        public Contract Contract{ get; set; }
    }
}
