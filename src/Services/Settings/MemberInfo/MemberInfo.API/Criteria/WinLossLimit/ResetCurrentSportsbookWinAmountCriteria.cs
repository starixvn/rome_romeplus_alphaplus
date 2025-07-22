using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class ResetCurrentSportsbookWinAmountCriteria : NonQueryCommand
    {
        public ResetCurrentSportsbookWinAmountCriteria(
            int custId,
            int sportType,
            int betType,
            int settingStatus)
        {
            CustId = custId;
            SportType = sportType;
            BetType = betType;
            SettingStatus = settingStatus;
        }

        public int CustId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public int SettingStatus { get; }

        public override string GetSettingKey()
            => "Age_BalanceGameReset_CashMember";

        public override bool IsValid() => CustId > 0;
    }
} 