namespace AlphaPlus.CashService.API.Models.Payments
{
    public class PaymentMethodModel
    {
        public PaymentMethodModel()
        {
            Name = string.Empty;
            APIMethodKey = string.Empty;
            MerchantID = string.Empty;
            AppSecret = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CustId { get; set; }
        public int ProviderId { get; set; }
        public int TransactionType { get; set; }
        public int Method { get; set; }
        public int Status { get; set; }

        public string APIMethodKey { get; set; }
        public string MerchantID { get; set; }
        public string AppSecret { get; set; }
    }
} 