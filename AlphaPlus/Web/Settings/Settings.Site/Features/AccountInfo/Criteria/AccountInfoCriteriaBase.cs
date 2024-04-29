using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class AccountInfoCriteriaBase(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public override string GetSettingKey() => string.Empty;

        public override bool IsValid() => CustId > 0;
    }
}