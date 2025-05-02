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
        public string TransactionTypeID { get; set; }
        public string TransactionTypes => TransactionTypeID;

        public string BankName { get; set; }
        public int BankId { get; set; }
        public int Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedStr => LastUpdated.ToString("MM/dd/yyyy hh:mm:ss tt");

        public int DepositMatchingMode { get; set; }
        public bool PromptPay { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string MobilePrefix { get; set; } = string.Empty;
        public string Creator { get; set; } = string.Empty;
        public int CreatorId { get; set; }
    }
}