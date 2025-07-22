using AlphaPlus.CashService.API.Features.Shared.Models.PaymentGateway;

namespace AlphaPlus.CashService.API.Features.Shared.Requests.PaymentGateway;

public class TestConnectionRequest
{
    public int ProviderId { get; set; }
    public int TransactionType { get; set; }
    public int PaymentMethod { get; set; }
    public APIConfigModel APIConfig { get; set; }
}