namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class BankModel
    {
        public string TransactionTypeID { get; set; }
        public string TransTypeList { get; set; }

        public string Bank { get; set; }
        public string DepositMode { get; set; }
        public string Code { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string TransactionTypes => TransactionTypeID ?? TransTypeList;
        public DepositMatchingModeModel DepositMatchingMode { get; set; }
    }
}