using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.ViewModels.Dashboard.Statistics;

namespace Alpha.Main.Site.Requests.Dashboard.Statistics
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