using System;

namespace AlphaPlus.CashService.API.Models.ManagePayments
{
    public class PaymentProviderFeeModel
    {
        public PaymentProviderFeeModel()
        {
            TransactionType = string.Empty;
            Method = string.Empty;
            Rate = 0;
        }

        public string TransactionType { get; set; }
        public string Method { get; set; }
        public decimal Rate { get; set; }
    }
} 