using System;

namespace AlphaPlus.CashService.API.Models.ManagePayments
{
    public class PaymentProviderModel
    {
        public PaymentProviderModel()
        {
            LastUpdated = DateTime.UtcNow;
            Name = string.Empty;
            Fee = 0m;
            Status = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public int Status { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 