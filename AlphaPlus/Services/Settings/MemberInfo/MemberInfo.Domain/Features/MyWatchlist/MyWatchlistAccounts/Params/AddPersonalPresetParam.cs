namespace AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params
{
    public class AddPersonalPresetParam
    {
        public AddPersonalPresetParam(int agentId, string presetTitle, decimal winningThreshold, decimal losingThreshold, string siteName)
        {
            AgentId = agentId;
            PresetTitle = presetTitle;
            WinningThreshold = winningThreshold;
            LosingThreshold = losingThreshold;
            SiteName = siteName;
        }

        public int AgentId { get; }

        public string PresetTitle { get; }

        public decimal WinningThreshold { get; }

        public decimal LosingThreshold { get; }

        public string SiteName { get; }
    }
}