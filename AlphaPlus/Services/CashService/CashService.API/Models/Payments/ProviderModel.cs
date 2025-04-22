using System;

namespace AlphaPlus.CashService.API.Models.Payments
{
    public class ProviderModel
    {
        public ProviderModel()
        {
            LastUpdated = DateTime.UtcNow;
            Name = string.Empty;
            Status = 0;
            CustId = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedStr => LastUpdated.ToString("MM/dd/yyyy hh:mm:ss tt");
        public int CustId { get; set; }
    }
} 