using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds.Criteria
{
    public class SyncUpdateMyThresholdCommand : NonQueryCommand
    {
        public SyncUpdateMyThresholdCommand(string custList, string suffix)
        {
            CustList = custList;
            Suffix = suffix;
        }

        public string CustList { get; }

        [SpParam(Ignored = true)]
        public string Suffix { get; }

        public override string GetSettingKey()
            => $"Acc_CustWLMonitored_Setting_Adjust_{Suffix}";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CustList);
    }
}
