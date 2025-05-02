namespace AlphaPlus.CashService.API.Models.Payments
{
    public class BankModel
    {
        public string Bank { get; set; }
        public string TransactionTypeID { get; set; }
        public int DepositMode { get; set; }

        public int Id { get; set; }
        public string Name => Bank;
        public string TransactionTypes => TransactionTypeID;
        public int DepositMatchingMode => DepositMode;
    }
}