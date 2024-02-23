using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.IPInfo
{
    public class GetIPInfoCriteria(string ip) : CriteriaBase
    {
        public string IP = ip;

        public override string GetSettingKey() => "Acc_GetIPInfo_Full";

        public override bool IsValid() => !string.IsNullOrEmpty(IP);
    }
}