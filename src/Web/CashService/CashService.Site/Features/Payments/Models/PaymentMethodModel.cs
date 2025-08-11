namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class PaymentMethodModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CustId { get; set; }
        public int ProviderId { get; set; }
        public int TransactionType { get; set; }
        public int Method { get; set; }
        public int Status { get; set; }

        public string APIMethodKey { get; set; } = string.Empty;
        public string MerchantID { get; set; } = string.Empty;
        public string AppSecret { get; set; } = string.Empty;
        public string APIConfig { get; set; }

        public static PaymentMethodModel CreateOptionAll()
        {
            return new PaymentMethodModel
            {
                Id = 0,
                Name = "All"
            };
        }
    }
}