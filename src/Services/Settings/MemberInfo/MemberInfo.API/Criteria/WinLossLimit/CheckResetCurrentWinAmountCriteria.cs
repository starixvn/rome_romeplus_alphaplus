using Fanex.Data.Repository;
using AlphaPlus.Settings.MemberInfo.Domain.Params;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class CheckResetCurrentWinAmountCriteria : NonQueryCommand
    {
        public CheckResetCurrentWinAmountCriteria()
        {
        }

        public CheckResetCurrentWinAmountCriteria(ResetCurrentWinAmountParam param)
        {
            CustId = param.CustId;
            ProductId = param.ProductId;
            SportType = param.SportType;
            BetType = param.BetType;
            SubAccId = param.SubAccId;
            SubAccName = param.SubAccName;
            UserId = param.UserId;
            SettingStatus = param.SettingStatus;
        }

        public int CustId { get; }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public int UserId { get; }

        public int SettingStatus { get; }

        public int ErrCode { get; set; }

        public string ErrMessage { get; set; }

        public override string GetSettingKey() => "Age_BalanceGameReset_CashMember_Check";

        public override bool IsValid() => CustId > 0;
    }
} 