namespace AlphaPlus.CashService.API.Models.PendingCommons;

public class MemberSettingDeposit
{
    public decimal AgentGivenCredit { get; set; }

    public decimal MemberUsedCredit { get; set; }

    public decimal MinLimitAmt { get; set; }

    public decimal MaxLimitAmt { get; set; }
}