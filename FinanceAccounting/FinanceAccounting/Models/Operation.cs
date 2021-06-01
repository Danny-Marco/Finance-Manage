using System;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceAccounting.Models.Interfaces;

namespace FinanceAccounting.Models
{
    public class Operation : IEntity
    {
        public int OperationId { get; set; }

        public int DefinitionId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Sum { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
        
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        public OperationEnum PurposeOperation
        {
            get => (OperationEnum) DefinitionId;
            set => DefinitionId = (int) value;
        }
    }
}