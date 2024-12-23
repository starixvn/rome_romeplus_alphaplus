using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal class GetRewardProgramsCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_Rpt_GetProgramList";

    public override bool IsValid() => true;
}