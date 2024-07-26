using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria;

public class GetMasterListCriteria(int custId) : CriteriaBase
{
    public int SRecommend { get; } = custId;

    public override string GetSettingKey() => "Age_CustomerSelMAgentList";

    public override bool IsValid() => SRecommend > 0;
}
