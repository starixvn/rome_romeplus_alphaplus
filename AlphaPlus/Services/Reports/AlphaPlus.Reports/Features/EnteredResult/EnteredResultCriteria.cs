using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.EnteredResult;

public class EnteredResultCriteria(bool includeTestData) : CriteriaBase
{
    public bool IncludeTestData { get; } = includeTestData;

    public override string GetSettingKey() => "Age_Rpt_MatchSelResultEntered";

    public override bool IsValid() => true;
}
