using Fanex.Data.Repository;

namespace AlphaPlus.Utilities.IP.Criteria;

public class GetIPInformationCriteria : CriteriaBase
{
    public GetIPInformationCriteria(string ip)
    {
        Ip = ip;
    }

    public string Ip { get; }

    public override string GetSettingKey() => "Acc_GetIPInfo_Full";

    public override bool IsValid() => !string.IsNullOrEmpty(Ip);
}