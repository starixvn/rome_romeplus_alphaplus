using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class ResetCurrentWinLimitAmountCriteria : NonQueryCommand
    {
        public ResetCurrentWinLimitAmountCriteria(
            int custId,
            int sportType,
            int betType)
        {
            CustId = custId;
            SportType = sportType;
            BetType = betType;
        }

        public int CustId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public override string GetSettingKey()
            => "Age_BalanceGameReset_CashMember";

        public override bool IsValid() => CustId > 0;
    }
} 