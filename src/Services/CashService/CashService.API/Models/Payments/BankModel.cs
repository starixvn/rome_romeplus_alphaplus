namespace AlphaPlus.CashService.API.Models.Payments
{
    public class BankModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TransactionTypes { get; set; } = string.Empty;
        public int DepositMatchingMode { get; set; }
    }
}