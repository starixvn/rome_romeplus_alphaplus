using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.Accounts.Criteria
{
    public class SelectCreditInfoCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public override string GetSettingKey() => "Age_Customer_CreditInfo_Sel";

        public override bool IsValid() => CustId > 0;
    }
}