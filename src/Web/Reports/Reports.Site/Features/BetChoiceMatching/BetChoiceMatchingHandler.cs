using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetChoiceMatching;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Constants;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingHandler(
    IReportDateService reportDateService,
    IBetChoiceMatchingService betChoiceMatchingService,
    ICacheRefService cacheRefService,
    ILeagueService leagueService,
    IBetChoiceMatchingWorkbook workbook) :
        IRequestHandler<BetChoiceMatchingQuery, BetChoiceMatchingViewModel>,
        IRequestHandler<BetChoiceMatchingJsonQuery, BetChoiceMatchingJsonModel>,
        IRequestHandler<BetChoiceUplinesJsonQuery, BetChoiceMatchingUplinesModel>,
        IRequestHandler<BetChoiceMatchingExcelQuery, ExcelFileViewModel>
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IBetChoiceMatchingService betChoiceMatchingService = betChoiceMatchingService;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly ILeagueService leagueService = leagueService;
    private readonly IBetChoiceMatchingWorkbook workbook = workbook;

    public async Task<BetChoiceMatchingViewModel> Handle(BetChoiceMatchingQuery message, CancellationToken cancellationToken)
        => new BetChoiceMatchingViewModel(message, await reportDateService.GetBalanceUpDay());

    public async Task<BetChoiceMatchingJsonModel> Handle(BetChoiceMatchingJsonQuery message, CancellationToken cancellationToken)
    {
        var param = new BetChoiceMatchingParam(
            message.ViewerId,
            message.EventDate,
            message.BetCount,
            message.IsAdminCanViewVNIP,
            message.Site,
            message.SRecommendId,
            message.System);
        var (events, ips) = await betChoiceMatchingService.GetBetChoiceMatching(param);

        return BuildBetChoiceMatchingJsonModel(events, ips);
    }

    private BetChoiceMatchingJsonModel BuildBetChoiceMatchingJsonModel(
        IEnumerable<BetChoiceMatchingEventModel> events,
        IEnumerable<BetChoiceMatchingSharedIPModel> ips)
    {
        var teamHash = new Dictionary<int, string>();
        var leagueGroupHash = new HashSet<string>();
        var leagueHash = new Dictionary<int, string>();
        var sportTypeHash = new Dictionary<int, string>();
        var betTypeHash = new Dictionary<int, string>();

        foreach (var item in events)
        {
            if (!teamHash.ContainsKey(item.HomeId))
            {
                teamHash.Add(item.HomeId, cacheRefService.GetTeamName(item.HomeId));
            }

            if (!teamHash.ContainsKey(item.AwayId))
            {
                teamHash.Add(item.AwayId, cacheRefService.GetTeamName(item.AwayId));
            }

            var leagueGroupKey = $"{item.LeagueId}_{item.LeagueGroupId}";
            if (leagueGroupHash.Add(leagueGroupKey))
            {
                leagueHash.Add(item.LeagueId, leagueService.GetLeagueName(item.LeagueId, LeagueType.Self, item.LeagueGroupId));
            }

            if (!sportTypeHash.ContainsKey(item.SportType))
            {
                sportTypeHash.Add(item.SportType, cacheRefService.GetSportName(item.SportType));
            }

            if (!betTypeHash.ContainsKey(item.BetType))
            {
                betTypeHash.Add(item.BetType, cacheRefService.GetBetTypeName(item.BetType));
            }
        }

        return new BetChoiceMatchingJsonModel(events, teamHash, leagueHash, sportTypeHash, betTypeHash, ips);
    }

    public Task<BetChoiceMatchingUplinesModel> Handle(BetChoiceUplinesJsonQuery message, CancellationToken cancellationToken)
        => betChoiceMatchingService.QueryUplines(message.CustId, message.Site);

    public async Task<ExcelFileViewModel> Handle(BetChoiceMatchingExcelQuery message, CancellationToken cancellationToken)
    {
        var param = new BetChoiceMatchingExcelParam(
            message.ViewerId,
            message.EventDate,
            message.BetCount,
            message.IsAdminCanViewVNIP,
            message.Site,
            message.SRecommendId,
            message.System,
            message.Username,
            message.Stake,
            message.MemberWinLossOperator,
            message.MemberWinLossValue);
        var events = await betChoiceMatchingService.GetBetChoiceMatchingExcel(param);

        return workbook.BuildExcelFile(message, events);
    }
}