using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.Dashboard.Statistics
{
    public class TopTurnoverMembersQuery : IRequest<TopTurnoverMembersViewModel>
    {
        public TopTurnoverMembersQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }
    }
}