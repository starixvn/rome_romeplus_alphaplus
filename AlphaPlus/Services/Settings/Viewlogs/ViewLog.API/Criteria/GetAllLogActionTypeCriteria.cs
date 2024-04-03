namespace AlphaPlus.Settings.ViewLogs.API.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllLogActionTypeCriteria : CriteriaBase
    {
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
