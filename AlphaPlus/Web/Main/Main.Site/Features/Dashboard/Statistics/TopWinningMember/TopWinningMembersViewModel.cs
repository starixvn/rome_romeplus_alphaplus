namespace AlphaPlus.Main.Site.Features.Dashboard.Statistics
{
    public class TopWinningMembersViewModel(IEnumerable<TopWinningMember> members)
    {
        public IEnumerable<TopWinningMember> Members { get; } = members;
    }
}