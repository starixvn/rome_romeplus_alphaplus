using Fanex.Data.Repository;

namespace AlphaPlus.Utilities.IP.Criteria
{
    public class GetIPVNCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "GetIPVN";

        public override bool IsValid() => true;
    }
}