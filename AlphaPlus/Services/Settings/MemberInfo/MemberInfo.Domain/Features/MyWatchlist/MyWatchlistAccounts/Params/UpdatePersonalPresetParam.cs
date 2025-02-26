namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class UpdatePersonalPresetParam
    {
        public UpdatePersonalPresetParam(int id, int agentId, string presetTitle, decimal winningThreshold, decimal losingThreshold)
        {
            ID = id;
            AgentId = agentId;
            PresetTitle = presetTitle;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
        }

        public int ID { get; }

        public int AgentId { get; }

        public string PresetTitle { get; }

        public decimal WinningThreshold { get; }

        public decimal LosingThreshold { get; }
    }
}
