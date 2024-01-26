using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CheckShowConsolidateReportCommand(int custId) : INonQueryCommand
    {
        public int CustId { get; } = custId;

        public bool IsShownMenu { get; set; }

        public string GetSettingKey() => "Age_Rpt_ConsolidatedGroup_isShownMenu";

        public bool IsValid() => CustId > 0;
    }
}