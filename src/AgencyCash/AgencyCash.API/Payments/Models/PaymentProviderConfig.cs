namespace AgencyCash.API.Payments.Models;

public class PaymentProviderConfig
{
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string BaseUrl { get; set; }
    public bool IsEnabled { get; set; }
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal Fee { get; set; }
    public string FeeType { get; set; } // "percentage" or "fixed"
} 