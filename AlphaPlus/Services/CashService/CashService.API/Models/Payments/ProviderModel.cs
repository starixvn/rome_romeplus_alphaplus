using System;
using System.Collections.Generic;

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
            ProviderSettings = new List<ProviderSettingsModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CustId { get; set; }
        public int Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public List<ProviderSettingsModel> ProviderSettings { get; set; }
        
        public string LastUpdatedStr => LastUpdated.ToString("MM/dd/yyyy hh:mm:ss tt");
    }
} 