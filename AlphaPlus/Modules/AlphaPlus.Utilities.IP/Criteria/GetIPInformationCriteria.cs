using Fanex.Data.Repository;

namespace AlphaPlus.Utilities.IP.Criteria;

public class GetIPInformationCriteria(string ip) : CriteriaBase
{
    public string Ip { get; set; } = ip;

    public override string GetSettingKey() => "Acc_GetIPInfo_Full";

    public override bool IsValid() => !string.IsNullOrEmpty(Ip);
}