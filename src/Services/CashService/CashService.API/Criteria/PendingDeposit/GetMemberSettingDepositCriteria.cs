using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit;

public class GetMemberSettingDepositCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public override string GetSettingKey() => "Age_CashDeposit_MemberSettingSel";
    public override bool IsValid() => CustId > 0;
}