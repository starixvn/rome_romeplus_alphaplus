using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class SetTransactionLimitCriteria : NonQueryCommand
    {
        public SetTransactionLimitCriteria(
            decimal minPerDepositTransaction,
            decimal maxPerDepositTransaction,
            decimal minPerWithdrawalTransaction,
            decimal maxPerWithdrawalTransaction,
            int dailyWithdrawalCount,
            int custId)
        {
            MinPerDepositTransaction = minPerDepositTransaction;
            MaxPerDepositTransaction = maxPerDepositTransaction;
            MinPerWithdrawalTransaction = minPerWithdrawalTransaction;
            MaxPerWithdrawalTransaction = maxPerWithdrawalTransaction;
            DailyWithdrawalCount = dailyWithdrawalCount;
            CustId = custId;
        }

        [SpParam(SqlParamName = "MinPerDepositTransaction")]
        public decimal MinPerDepositTransaction { get; }

        [SpParam(SqlParamName = "MaxPerDepositTransaction")]
        public decimal MaxPerDepositTransaction { get; }

        [SpParam(SqlParamName = "MinPerWithdrawalTransaction")]
        public decimal MinPerWithdrawalTransaction { get; }

        [SpParam(SqlParamName = "MaxPerWithdrawalTransaction")]
        public decimal MaxPerWithdrawalTransaction { get; }

        [SpParam(SqlParamName = "DailyWithdrawalCount")]
        public int DailyWithdrawalCount { get; }

        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_SetTransactionLimit";

        public override bool IsValid() => 
            MinPerDepositTransaction >= 0 && 
            MaxPerDepositTransaction >= MinPerDepositTransaction &&
            MinPerWithdrawalTransaction >= 0 && 
            MaxPerWithdrawalTransaction >= MinPerWithdrawalTransaction &&
            DailyWithdrawalCount >= 0 && 
            CustId > 0;
    }
} 