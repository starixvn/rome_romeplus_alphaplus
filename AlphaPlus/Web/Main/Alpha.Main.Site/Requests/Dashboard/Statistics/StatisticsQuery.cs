using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.ViewModels.Dashboard.Statistics;

namespace Alpha.Main.Site.Requests.Dashboard.Statistics
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