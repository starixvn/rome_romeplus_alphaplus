using Alpha.Main.Models.Statistics;

namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    public class TopTurnoverMembersViewModel(IEnumerable<TopTurnoverMember> members)
    {
        public IEnumerable<TopTurnoverMember> Members { get; } = members;
    }
}