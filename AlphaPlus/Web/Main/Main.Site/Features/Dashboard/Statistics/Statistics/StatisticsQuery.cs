using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.Dashboard.Statistics
{
    public class StatisticsQuery : IRequest<StatisticsViewModel>
    {
        public StatisticsQuery(IUserSession userSession)
        {
            CustId = userSession.CustId;
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            UserSession = userSession;
        }

        public int CustId { get; }

        public UserLevel UserLevel { get; }

        public IUserSession UserSession { get; }
    }
}