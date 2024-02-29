namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
    using Fanex.Data.Repository;

    public class GetWinLossLimitProductCriteria : CriteriaBase
    {
        public GetWinLossLimitProductCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey()
            => "Age_CustomerSelWinLimitProduct";

        public override bool IsValid()
            => CustId > 0;
    }
}
