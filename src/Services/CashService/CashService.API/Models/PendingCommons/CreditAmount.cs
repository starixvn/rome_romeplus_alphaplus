namespace AlphaPlus.CashService.API.Models.PendingCommons;

public class CreditAmount
{
    public decimal MinAmt { get; set; }
    public decimal MaxAmt { get; set; }
    public bool IsReachLimitCnt { get; set; }
}