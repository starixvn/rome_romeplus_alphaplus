using Fanex.Data.Repository;

namespace AlphaPlus.Settings.ViewLogs.API.Criteria
{
    public class GetLogActionsCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_UserLog_LogActionSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
