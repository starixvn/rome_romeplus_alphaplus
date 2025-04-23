using System;

namespace AlphaPlus.CashService.Site.Features.Payments.Models
{
    public class ProviderSettingsModel
    {
        public int CustId { get; set; }
        public int ProviderId { get; set; }
        public int Method { get; set; }
        public string MethodName { get; set; }
        public int Status { get; set; }
        public int TransactionType { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal MinAmountTrans { get; set; }
        public decimal MaxAmountTrans { get; set; }
        public bool IsEnabled { get; set; }
        public bool BankRequired { get; set; }
    }
} 