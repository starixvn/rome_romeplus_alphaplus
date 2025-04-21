using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class SetDailyWithdrawalCountCriteria : NonQueryCommand
    {
        public SetDailyWithdrawalCountCriteria(int dailyWithdrawalCount, int custId)
        {
            DailyWithdrawalCount = dailyWithdrawalCount;
            CustId = custId;
        }

        public int DailyWithdrawalCount { get; }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_SetDailyWithdrawalCount";

        public override bool IsValid() => DailyWithdrawalCount >= 0 && CustId > 0;
    }
} 