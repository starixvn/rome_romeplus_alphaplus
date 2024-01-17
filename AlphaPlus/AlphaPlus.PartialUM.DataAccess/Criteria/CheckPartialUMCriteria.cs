using Fanex.Data.Repository;

namespace AlphaPlus.PartialUM.DataAccess.Criteria
{
    internal class CheckPartialUMCriteria : CriteriaBase
    {
        public CheckPartialUMCriteria(string siteName)
            => SiteName = siteName;

        public string SiteName { get; }

        public override string GetSettingKey() => "Acc_CheckUM_ByReport";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(SiteName);
    }
}
