namespace Alpha.Main.Site.Features.Dashboard.Statistics
{
    public class NewMembersViewModel(IEnumerable<NewMember> members)
    {
        public IEnumerable<NewMember> Members { get; } = members;
    }
}