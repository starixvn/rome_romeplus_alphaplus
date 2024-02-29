namespace AlphaPlus.Settings.MemberInfo.API.Features.ParlayMaxPayout.Criteria
{
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
    using Fanex.Data.Repository;

    public class ParlayMaxPayoutSelectCriteria : CriteriaBase
    {
        public ParlayMaxPayoutSelectCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Acc_CustSetting_MaxPayoutSelect";

        public override bool IsValid() => CustId > 0;
    }
}