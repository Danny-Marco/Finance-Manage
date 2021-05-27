using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FinanceAccounting.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        
        public decimal CurrentSum { get; set; }
        
        [JsonIgnore] 
        [IgnoreDataMember]
        public virtual List<Operation> ? Operations { get; set; }
    }
}