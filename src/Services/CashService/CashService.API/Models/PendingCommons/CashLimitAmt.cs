namespace AlphaPlus.CashService.API.Models.PendingCommons;

using Fanex.Data;

public class CashLimitAmt
{
    public int CustId { get; set; }

    public int ProviderId { get; set; }

    public int UplineId { get; set; }

    public decimal CashBalance { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }
}