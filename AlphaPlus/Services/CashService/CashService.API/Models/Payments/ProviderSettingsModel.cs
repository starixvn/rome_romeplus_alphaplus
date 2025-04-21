namespace AlphaPlus.CashService.API.Models.Payments
{
    public class ProviderSettingsModel
    {
        public int ProviderId { get; set; }
        public int Method { get; set; }
        public int Status { get; set; }
        public int TransactionType { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public bool IsEnabled { get; set; }
        public bool BankRequired { get; set; }
    }
} 