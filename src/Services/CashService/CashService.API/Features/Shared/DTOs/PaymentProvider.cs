namespace AlphaPlus.CashService.API.Features.Shared.DTOs;

public class PaymentProvider
{
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public int PaymentMethodId { get; set; }
    public string MethodName { get; set; }
}