using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;

public class GetMemberSettingWithdrawCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public override string GetSettingKey() => "Age_CashWithdrawal_MemberSettingSel";
    public override bool IsValid() => CustId > 0;
}