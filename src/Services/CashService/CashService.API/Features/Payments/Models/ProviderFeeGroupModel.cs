namespace AlphaPlus.CashService.API.Features.Payments.Models
{
    public class ProviderFeeGroupModel
    {
        public string TransactionType { get; set; } = string.Empty;
        public List<ProviderFeeModel> Fees { get; set; } = new();
    }
}