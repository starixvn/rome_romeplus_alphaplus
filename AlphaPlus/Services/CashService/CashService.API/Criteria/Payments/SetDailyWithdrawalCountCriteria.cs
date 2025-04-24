using AlphaPlus.CashService.API.Constants;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class SetDailyWithdrawalCountCriteria : NonQueryCommand
    {
        public SetDailyWithdrawalCountCriteria(int custId, int dailyWithdrawalCount)
        {
            CustId = custId;
            DailyWithdrawalCount = dailyWithdrawalCount;
        }

        public int CustId { get; }

        public int ModeAction { get; } = ModeViewConstants.PaymentGateway; 

        [SpParam(SqlParamName = "DrawLimit")]
        public int DailyWithdrawalCount { get; } 

        public decimal MinPerDeposit { get; } = 0;
        public decimal MaxPerDeposit { get; } = 0;
        public decimal MinPerWithdrawal { get; } = 0;
        public decimal MaxPerWithdrawal { get; } = 0;

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_DailyWithdrawLimit_Upd";

        public override bool IsValid() => DailyWithdrawalCount >= 0 && CustId > 0;
    }
} 