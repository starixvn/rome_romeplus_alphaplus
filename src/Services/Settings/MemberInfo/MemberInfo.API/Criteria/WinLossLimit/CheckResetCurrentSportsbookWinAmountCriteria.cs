using AlphaPlus.Settings.MemberInfo.Domain.Params;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class CheckResetCurrentSportsbookWinAmountCriteria : NonQueryCommand
    {
        public CheckResetCurrentSportsbookWinAmountCriteria()
        {
        }

        public CheckResetCurrentSportsbookWinAmountCriteria(ResetCurrentSportsbookWinAmountParam param)
        {
            CustId = param.CustId;
            SubAccId = param.SubAccId;
            SubAccName = param.SubAccName;
            UserId = param.UserId;
            SettingStatus = param.SettingStatus;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public int UserId { get; }

        public int SettingStatus { get; }

        public int ErrCode { get; set; }

        public string ErrMessage { get; set; }
        public override string GetSettingKey()
            => "Acc_CustWinLimitBetReset_CashMember_Check";

        public override bool IsValid() => CustId > 0;
    }
} 