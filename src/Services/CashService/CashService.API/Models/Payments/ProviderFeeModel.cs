namespace AlphaPlus.CashService.API.Models.Payments
{
    public class ProviderFeeModel
    {
        public int ProviderId { get; set; } = 0;
        public string TransactionType { get; set; } = string.Empty;
        public int MethodId { get; set; }
        public string Method { get; set; } = string.Empty;
        public decimal Rate { get; set; } = 0;
        public string RateText => Rate.ToString("#,##0.##") + "%";
    }
}