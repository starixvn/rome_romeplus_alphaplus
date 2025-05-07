using AlphaPlus.CashService.API.Constants;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class SetTransactionLimitCriteria(
        int custId,
        int dailyWithdrawalCount,
        decimal minPerDeposit,
        decimal maxPerDeposit,
        decimal minPerWithdrawal,
        decimal maxPerWithdrawal)
        : NonQueryCommand
    {
        public int CustId { get; } = custId;
        public int ModeAction { get; } = ModeViewConstants.BankAccount; 

        [SpParam(SqlParamName = "DrawLimit")]
        public int DailyWithdrawalCount { get; } = dailyWithdrawalCount;

        public decimal MinPerDeposit { get; } = minPerDeposit;
        public decimal MaxPerDeposit { get; } = maxPerDeposit;
        public decimal MinPerWithdrawal { get; } = minPerWithdrawal;
        public decimal MaxPerWithdrawal { get; } = maxPerWithdrawal;

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_AgentBankAccountLimit_Upd";

        public override bool IsValid() => 
            DailyWithdrawalCount >= 0 && 
            CustId > 0 &&
            MinPerDeposit >= 0 &&
            MaxPerDeposit >= 0 &&
            MinPerWithdrawal >= 0 &&
            MaxPerWithdrawal >= 0;
    }
} 