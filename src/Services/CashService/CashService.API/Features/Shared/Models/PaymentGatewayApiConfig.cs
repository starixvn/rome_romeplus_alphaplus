namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class PaymentGatewayApiConfig
{
    public bool Enabled { get; set; }
    public string BaseUrl { get; set; }
    public string MerchantNo { get; set; }
    public string AppSecret { get; set; }
    public string SecretKey { get; set; }
}