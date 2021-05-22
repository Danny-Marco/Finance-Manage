using System.Collections.Generic;

namespace FinanceAccounting.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        
        public decimal CurrentSum { get; set; }
        
        public virtual List<Operation> ? Operations { get; set; }
    }
}