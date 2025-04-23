using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.Dashboard.Statistics
{
    public class TopWinningMembersQuery : IRequest<TopWinningMembersViewModel>
    {
        public TopWinningMembersQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }
    }
}