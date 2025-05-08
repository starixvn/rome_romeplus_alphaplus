using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Transactions;

public class GetMembersCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public override string GetSettingKey() => "Age_CashService_MemberListSel";

    public override bool IsValid() => true;
}