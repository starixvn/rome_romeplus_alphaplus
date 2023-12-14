using Alpha.Main.Models.Statistics;

namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    public class NewMembersViewModel(IEnumerable<NewMember> members)
    {
        public IEnumerable<NewMember> Members { get; } = members;
    }
}