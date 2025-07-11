namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class PaymentGatewayApiConfig
{
    public bool Enabled { get; set; }
    public string BaseUrl { get; set; }
    public bool IsTestMode { get; set; }
}