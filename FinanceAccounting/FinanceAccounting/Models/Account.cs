using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using FinanceAccounting.Models.Interfaces;
using Newtonsoft.Json;

namespace FinanceAccounting.Models
{
    public class Account : IEntity
    {
        public int AccountId { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CurrentSum { get; set; }
        
        [JsonIgnore] 
        [IgnoreDataMember]
        public virtual List<Operation> ? Operations { get; set; }
    }
}