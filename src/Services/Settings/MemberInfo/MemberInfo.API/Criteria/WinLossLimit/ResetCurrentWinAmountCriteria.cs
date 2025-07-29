using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class ResetCurrentWinAmountCriteria : NonQueryCommand
    {
        public ResetCurrentWinAmountCriteria(
            int custId,
            int userId,
            int sportType,
            int betType,
            int settingStatus)
        {
            CustId = custId;
            SportType = sportType;
            BetType = betType;
            SettingStatus = settingStatus;
            UserId = userId;
        }

        public int CustId { get; }

        public int UserId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public int SettingStatus { get; }

        public override string GetSettingKey()
            => "Age_BalanceGameReset_CashMember";

        public override bool IsValid() => CustId > 0;
    }
} 