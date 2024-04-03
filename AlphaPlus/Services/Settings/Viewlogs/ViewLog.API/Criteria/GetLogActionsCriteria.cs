namespace AlphaPlus.Settings.ViewLogs.API.Criteria
{
    using Fanex.Data.Repository;

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
