namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data.Repository;

public class GetMembersCriteria : CriteriaBase
{
    public long CustId { get; set; }

    public override string GetSettingKey() => "Age_CashService_MemberListSel";

    public override bool IsValid() => CustId > 0;
}