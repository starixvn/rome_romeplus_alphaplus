namespace AlphaPlus.CashService.API.Features.Transactions.Models;

public class GetCashLimitAmt
{
    public int CustId { get; set; }
    public int ProviderId { get; set; }
    public int UplineId { get; set; }
    public int CustDbGroupId { get; set; }
    public int PaymentMethodId { get; set; }
}