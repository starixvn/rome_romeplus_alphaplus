namespace AlphaPlus.CashService.API.Models.Payments
{
    public class ProviderFeeGroupModel
    {
        public string TransactionType { get; set; } = string.Empty;
        public List<ProviderFeeModel> Fees { get; set; } = new();
    }
}