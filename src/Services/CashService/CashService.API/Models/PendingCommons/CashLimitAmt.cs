namespace AlphaPlus.CashService.API.Models.PendingCommons;

public class CashLimitAmt
{
    public int CustId { get; set; }

    public int ProviderId { get; set; }

    public int UplineId { get; set; }

    public decimal CashBalance { get; set; }

}