using System;

namespace AlphaPlus.CashService.API.Models.Payments
{
    public class BankModel
    {
        public BankModel()
        {
            Name = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
} 