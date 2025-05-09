namespace AlphaPlus.CashService.API.Models.PendingCommons;

public class MemberSettingWithdraw
{
    public int WithdrawalLimitCnt { get; set; }

    public decimal MinLimitAmt { get; set; }

    public decimal MaxLimitAmt { get; set; }
}
