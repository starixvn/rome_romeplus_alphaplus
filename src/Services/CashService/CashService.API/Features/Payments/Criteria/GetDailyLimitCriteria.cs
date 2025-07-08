using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetDailyLimitCriteria(int custId, int modeView) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public int ModeView { get; } = modeView;

        public override string GetSettingKey() => "Age_CashService_AgentBankAccountLimit_Sel";

        public override bool IsValid() => CustId > 0;
    }
} 