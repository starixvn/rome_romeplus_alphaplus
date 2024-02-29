
namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    using Fanex.Data.Repository;

    public class AddPersonalPresetCommand : NonQueryCommand
    {
        public AddPersonalPresetCommand(int agentId, string presetTitle, decimal? winningThreshold, decimal? losingThreshold, int sporttype = 0)
        {
            AgentId = agentId;
            PresetTitle = presetTitle;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public string PresetTitle { get; }

        public decimal? WinningThreshold { get; }

        public decimal? LosingThreshold { get; }

        public int Sporttype { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Preset_Ins";

        public override bool IsValid() => AgentId > 0;
    }
}
