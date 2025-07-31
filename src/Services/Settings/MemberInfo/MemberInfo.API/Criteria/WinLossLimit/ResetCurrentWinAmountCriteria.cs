using Fanex.Data.Repository;
using AlphaPlus.Settings.MemberInfo.Domain.Params;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class ResetCurrentWinAmountCriteria : NonQueryCommand
    {
        public ResetCurrentWinAmountCriteria(ResetCurrentWinAmountParam param)
        {
            CustId = param.CustId;
            ProductId = param.ProductId;
            SportType = param.SportType;
            BetType = param.BetType;
            SubAccId = param.SubAccId;
            SubAccName = param.SubAccName;
            UserId = param.UserId;
            IP = param.IP;
            SettingStatus = param.SettingStatus;
        }

        public int CustId { get; }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public int UserId { get; }

        public string IP { get; }

        public int SettingStatus { get; }

        public override string GetSettingKey() => "Age_BalanceGameReset_CashMember";

        public override bool IsValid() => CustId > 0;
    }
} 