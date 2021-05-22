using System;

namespace FinanceAccounting.Models
{
    public class Operation
    {
        public int OperationId { get; set; }

        public int DefinitionId { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
        
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        public OperationEnum PurposeOperation
        {
            get => (OperationEnum) DefinitionId;
            set => DefinitionId = (int) value;
        }
    }
}