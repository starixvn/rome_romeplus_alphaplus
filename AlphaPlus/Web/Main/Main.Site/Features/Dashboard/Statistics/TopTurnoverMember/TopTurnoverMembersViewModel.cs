namespace AlphaPlus.Main.Site.Features.Dashboard.Statistics
{
    public class TopTurnoverMembersViewModel(IEnumerable<TopTurnoverMember> members)
    {
        public IEnumerable<TopTurnoverMember> Members { get; } = members;
    }
}