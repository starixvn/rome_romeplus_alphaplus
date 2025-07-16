namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class PaymentResponse
{
    public int Code { get; set; } = 1;
    public string Message { get; set; }
    public bool IsSuccessful => Code == 0;
}