using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class SelectScheduleOptionCommand : NonQueryCommand
    {
        public SelectScheduleOptionCommand(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public bool IsAllowAutoTransfer { get; set; }

        public string ScheduleWeekDays { get; set; }

        public override string GetSettingKey() => "Acc_CustomerSelScheduleOption";

        public override bool IsValid() => CustId > 0;
    }
}