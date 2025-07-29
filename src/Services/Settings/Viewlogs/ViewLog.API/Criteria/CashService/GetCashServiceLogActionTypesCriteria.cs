using Fanex.Data.Repository;

namespace AlphaPlus.Settings.ViewLogs.API.Criteria
{
    public class GetCashServiceLogActionTypesCriteria : CriteriaBase
    {
        public int ServiceType = 1;

        public override string GetSettingKey()
        {
            return "Acc_UserLog_LogActionTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
