using MediatR;

namespace Alpha.Main.Site.Features.Dashboard.Statistics
{
    public class NewMembersQuery : IRequest<NewMembersViewModel>
    {
        public NewMembersQuery(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}