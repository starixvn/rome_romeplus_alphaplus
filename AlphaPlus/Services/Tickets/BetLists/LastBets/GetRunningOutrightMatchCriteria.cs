using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public class GetRunningOutrightMatchCriteria : CriteriaBase
{
    public bool IsVSRacing { get; } = false;

    public override string GetSettingKey() => "Age_Monitor_OutrightSel";

    public override bool IsValid() => true;
}