namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Models.PaymentGateway;

public class BasePaymentGatewayRequest
{
    public AgentApiConfig ApiConfig { get; set; }
}

public class AgentApiConfig
{
    public string MerchantNo { get; set; }
    public string AppSecret { get; set; }
    public string SecretKey { get; set; }
}