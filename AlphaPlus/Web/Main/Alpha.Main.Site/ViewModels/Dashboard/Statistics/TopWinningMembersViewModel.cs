using Alpha.Main.Models.Statistics;

namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    public class TopWinningMembersViewModel(IEnumerable<TopWinningMember> members)
    {
        public IEnumerable<TopWinningMember> Members { get; } = members;
    }
}