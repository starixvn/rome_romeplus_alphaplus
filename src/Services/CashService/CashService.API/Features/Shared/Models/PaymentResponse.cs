namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class PaymentResponse<T>
{
    public int Code { get; set; } = 1;
    public string Message { get; set; }
    public bool IsSuccessful => Code == 0;
    public T Data { get; set; }
}

public class WithdrawalDto
{
    public long TransId { get; set; }
    public string ProviderTransId { get; set; }
}