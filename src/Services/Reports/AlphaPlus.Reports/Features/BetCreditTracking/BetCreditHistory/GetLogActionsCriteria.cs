using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

public class GetLogActionsCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Acc_UserLog_LogActionSel";

    public override bool IsValid() => true;
}
