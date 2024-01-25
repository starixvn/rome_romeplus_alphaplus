using Fanex.Data.Repository;

namespace AlphaPlus.Settings.SubAccount.Infrastruture
{
    public class SubAccountCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustSubSelInfo";
        }

        public override bool IsValid()
        {
            return CustId > 0 && SubAccId > 0;
        }
    }
}