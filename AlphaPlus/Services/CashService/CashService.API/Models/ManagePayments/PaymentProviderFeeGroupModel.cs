using System;
using System.Collections.Generic;

namespace AlphaPlus.CashService.API.Models.ManagePayments
{
    public class PaymentProviderFeeGroupModel
    {
        public PaymentProviderFeeGroupModel()
        {
            TransactionType = string.Empty;
            Fees = new List<PaymentProviderFeeModel>();
        }

        public string TransactionType { get; set; }
        public List<PaymentProviderFeeModel> Fees { get; set; }
    }
} 