namespace AlphaPlus.CashService.API.Models.Payments
{
    public class PaymentMethodModel
    {
        public PaymentMethodModel()
        {
            Name = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProviderId { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }
    }
} 