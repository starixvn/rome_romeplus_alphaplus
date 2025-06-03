namespace AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetMemberSettingDepositCriteria : CriteriaBase
{
    public int CustId { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public decimal AgentGivenCredit { get; set; }

    public decimal CreditMembers_GivenCredit { get; set; }

    public override string GetSettingKey() => $"CustDb_{CustDbGroupId}_Age_CashDeposit_MemberSettingSel";

    public override bool IsValid() => CustId > 0;
}