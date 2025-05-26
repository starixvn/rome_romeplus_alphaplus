namespace AlphaPlus.CashService.API.Features.PendingDeposit.Models;

public class MemberSettingDeposit
{
    public decimal AgentGivenCredit { get; set; }

    public decimal CreditMembers_GivenCredit { get; set; }

    public decimal MinLimitAmt { get; set; }

    public decimal? MaxLimitAmt { get; set; }
}