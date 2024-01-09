using Fanex.Data.Repository;

namespace AlphaPlus.RequestInfo.Criteria;

public class GetIPInformationCriteria : CriteriaBase
{
    public string Ip { get; init; }

    public override string GetSettingKey()
        => "Acc_GetIPInfo_Full";

    public override bool IsValid()
        => !string.IsNullOrEmpty(Ip);
}