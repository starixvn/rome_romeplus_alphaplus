using MediatR;

namespace Alpha.Main.Site.Features.Dashboard.Statistics
{
    public class StatisticsHandler(IStatisticsService statisticsService) :
        IRequestHandler<StatisticsQuery, StatisticsViewModel>,
        IRequestHandler<TopWinningMembersQuery, TopWinningMembersViewModel>,
        IRequestHandler<TopTurnoverMembersQuery, TopTurnoverMembersViewModel>,
        IRequestHandler<NewMembersQuery, NewMembersViewModel>
    {
        private readonly IStatisticsService statisticsService = statisticsService;

        public async Task<StatisticsViewModel> Handle(StatisticsQuery message, CancellationToken cancellationToken)
        {

            var statistics = await statisticsService.GetStatistics(message.CustId, message.UserLevel);

            var viewModel = new StatisticsViewModel(
                statistics,
                message.UserLevel,
                message.UserSession.SubPageAccess.ViewReports);

            return viewModel;
        }

        public async Task<TopWinningMembersViewModel> Handle(TopWinningMembersQuery message, CancellationToken cancellationToken)
        {
            var members = await statisticsService.GetTopWinningMembers(message.CustId, message.UserLevel);

            return new TopWinningMembersViewModel(members);
        }

        public async Task<TopTurnoverMembersViewModel> Handle(TopTurnoverMembersQuery message, CancellationToken cancellationToken)
        {
            var members = await statisticsService.GetTopTurnoverMembers(message.CustId, message.UserLevel);
            return new TopTurnoverMembersViewModel(members);
        }

        public async Task<NewMembersViewModel> Handle(NewMembersQuery message, CancellationToken cancellationToken)
        {
            var members = await statisticsService.GetNewMembers(message.CustId);

            var viewModel = new NewMembersViewModel(members);

            return viewModel;
        }
    }
}