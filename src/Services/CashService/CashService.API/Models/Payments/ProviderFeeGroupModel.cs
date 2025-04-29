namespace AlphaPlus.CashService.API.Models.Payments
{
    public class ProviderFeeGroupModel
    {
        public ProviderFeeGroupModel()
        {
            TransactionType = string.Empty;
            Fees = new List<ProviderFeeModel>();
        }

        public string TransactionType { get; set; }
        public List<ProviderFeeModel> Fees { get; set; }
    }
}