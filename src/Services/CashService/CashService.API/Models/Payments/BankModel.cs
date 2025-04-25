using System;
using System.Collections.Generic;

namespace AlphaPlus.CashService.API.Models.Payments
{
    public class BankModel
    {
        public BankModel()
        {
            TransactionType = new List<TransactionTypeModel>();
            DepositMatchingMode = new List<int>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<TransactionTypeModel> TransactionType { get; set; }
        public List<int> DepositMatchingMode { get; set; }
    }
} 