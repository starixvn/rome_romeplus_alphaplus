using System;

namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class ProviderFeeModel
    {
        public ProviderFeeModel()
        {
            TransactionType = string.Empty;
            Method = string.Empty;
            Rate = 0;
            ProviderId = 0;
        }

        public int ProviderId { get; set; }
        public string TransactionType { get; set; }
        public string Method { get; set; }
        public decimal Rate { get; set; }
    }
} 