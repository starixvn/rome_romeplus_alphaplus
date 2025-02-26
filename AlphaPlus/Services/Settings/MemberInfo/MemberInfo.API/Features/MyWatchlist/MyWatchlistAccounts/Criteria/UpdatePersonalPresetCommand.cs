using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria
{
    public class UpdatePersonalPresetCommand : NonQueryCommand
    {
        public UpdatePersonalPresetCommand(int agentId, int id, string presetTitle, decimal winningThreshold, decimal losingThreshold, int sporttype = 0)
        {
            AgentId = agentId;
            ID = id;
            PresetTitle = presetTitle;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            Sporttype = sporttype;
        }

        public int AgentId { get; }

        public int ID { get; }

        public string PresetTitle { get; }

        public decimal WinningThreshold { get; }

        public decimal LosingThreshold { get; }

        public int Sporttype { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_WLMonitored_Preset_Upd";

        public override bool IsValid() => AgentId > 0;
    }
}
