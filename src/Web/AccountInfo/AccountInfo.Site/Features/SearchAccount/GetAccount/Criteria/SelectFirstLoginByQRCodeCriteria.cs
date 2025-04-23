using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Criteria
{
    public class SelectFirstLoginByQRCodeCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public override string GetSettingKey() => "Age_CheckQRFirstLogin";

        public override bool IsValid() => CustId > 0;
    }
}