using AlphaPlus.CashService.API.Constants;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class SetTransactionLimitCriteria : NonQueryCommand
    {
        public SetTransactionLimitCriteria(
            int custId, 
            int dailyWithdrawalCount,
            decimal minPerDeposit,
            decimal maxPerDeposit,
            decimal minPerWithdrawal,
            decimal maxPerWithdrawal)
        {
            CustId = custId;
            DailyWithdrawalCount = dailyWithdrawalCount;
            MinPerDeposit = minPerDeposit;
            MaxPerDeposit = maxPerDeposit;  
            MinPerWithdrawal = minPerWithdrawal;
            MaxPerWithdrawal = maxPerWithdrawal;
        }

        public int CustId { get; }
        public int ModeAction { get; } = ModeViewConstants.BankAccount; 

        [SpParam(SqlParamName = "DrawLimit")]
        public int DailyWithdrawalCount { get; }
        public decimal MinPerDeposit { get; }
        public decimal MaxPerDeposit { get; } 
        public decimal MinPerWithdrawal { get; } 
        public decimal MaxPerWithdrawal { get; } 

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