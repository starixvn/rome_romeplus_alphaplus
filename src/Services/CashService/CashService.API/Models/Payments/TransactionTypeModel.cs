using System;

namespace AlphaPlus.CashService.API.Models.Payments
{
    public class TransactionTypeModel
    {
        public TransactionTypeModel()
        {
            Name = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeposit { get; set; }
    }
} 