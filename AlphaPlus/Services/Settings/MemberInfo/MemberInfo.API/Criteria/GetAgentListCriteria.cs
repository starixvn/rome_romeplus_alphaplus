using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria;

public class GetAgentListCriteria(int custId) : CriteriaBase
{
    public int MRecommend { get; } = custId;

    public override string GetSettingKey() => "Age_CustomerSelAgentList";

    public override bool IsValid() => MRecommend > 0;
}
