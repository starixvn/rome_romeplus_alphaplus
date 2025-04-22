using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class SetDailyWithdrawalCountCriteria : NonQueryCommand
    {
        public SetDailyWithdrawalCountCriteria(int custId, int modeAction,  int dailyWithdrawalCount)
        {
            CustId = custId;
            ModeAction = modeAction;
            DailyWithdrawalCount = dailyWithdrawalCount;
        }

        public int CustId { get; }

        public int ModeAction { get; }

        [SpParam(SqlParamName = "DrawLimit")]
        public int DailyWithdrawalCount { get; }

        public int MinPerDeposit { get; } = 0;
        public int MaxPerDeposit { get; } = 0;
        public int MinPerWithdrawal { get; } = 0;
        public int MaxPerWithdrawal { get; } = 0;

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_DailyWithdrawLimit_Upd";

        public override bool IsValid() => DailyWithdrawalCount >= 0 && CustId > 0;
    }
} 