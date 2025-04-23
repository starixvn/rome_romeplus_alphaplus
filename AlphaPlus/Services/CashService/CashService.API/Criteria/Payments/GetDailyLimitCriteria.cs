using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetDailyLimitCriteria : CriteriaBase
    {
        public GetDailyLimitCriteria(int custId, int modeView)
        {
            CustId = custId;
            ModeView = modeView;
        }

        public int CustId { get; }

        public int ModeView { get; set; }

        public override string GetSettingKey() => "Age_CashService_DailyWithdrawLimit_Sel";

        public override bool IsValid() => CustId > 0;
    }
} 