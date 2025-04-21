using System;
using System.Collections.Generic;

namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class ProviderFeeGroupModel
    {
        public ProviderFeeGroupModel()
        {
            TransactionType = string.Empty;
            Fees = new List<ProviderFeeModel>();
        }

        public string TransactionType { get; set; }
        public List<ProviderFeeModel> Fees { get; set; }
    }
} 