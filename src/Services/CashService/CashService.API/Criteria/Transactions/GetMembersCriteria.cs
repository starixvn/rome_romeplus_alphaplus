namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetMembersCriteria : CriteriaBase
{
    public int CustId { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public override string GetSettingKey() => $"Cust_{CustDbGroupId}_Age_CashService_MemberListSel";

    public override bool IsValid() => CustId > 0;
}