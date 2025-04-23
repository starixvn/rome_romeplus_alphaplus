using System;

namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class PaymentMethodModel
    {
        public PaymentMethodModel()
        {
            Name = string.Empty;
            Status = 0;
            ProviderId = 0;
            TransactionType = 0;
            Creator = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProviderId { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }

        public static PaymentMethodModel CreateAllOption()
        {
            return new PaymentMethodModel
            {
                Id = 0,
                Name = "All"
            };
        }
    }
} 