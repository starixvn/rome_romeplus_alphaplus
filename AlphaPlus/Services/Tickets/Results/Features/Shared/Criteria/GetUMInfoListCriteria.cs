using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Shared;

internal class GetUMInfoListCriteria(string siteName) : CriteriaBase
{
    public string SiteName { get; } = siteName;

    public override string GetSettingKey() => "Acc_CheckUM_ByReport";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(SiteName);
}