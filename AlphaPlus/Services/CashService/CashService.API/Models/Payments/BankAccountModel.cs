using System;

namespace AlphaPlus.CashService.API.Models.Payments
{
    public class BankAccountModel
    {
        public BankAccountModel()
        {
            AccountNickname = string.Empty;
            AccountNumber = string.Empty;
            AccountHolderName = string.Empty;
            LastUpdated = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountNickname { get; set; } = string.Empty;
        public string AccountHolderName { get; set; } = string.Empty;
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public BankModel Bank { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedStr => LastUpdated.ToString("MM/dd/yyyy hh:mm:ss tt");
    }
} 