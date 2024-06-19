using AlphaPlus.BetLists.LastBets;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Utils;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.LastBets;

public class LastBetsHandler(
    ILastBetsService lastBetsService,
    IFilterBetsService filterBetsService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    IForecastService forecastService,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IEncryption encryption,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<LastBetsQuery, LastBetsViewModel>,
        IRequestHandler<GetLastBetsDataQuery, LastBetsDataViewModel>,
        IRequestHandler<LastBetsNewQuery, LastBetsNewViewModel>,
        IRequestHandler<GetLastBetsDataNewQuery, LastBetsDataNewViewModel>
{
    private const int NumberOfEncodeResult = 6;
    private readonly ILastBetsService lastBetsService = lastBetsService;
    private readonly IFilterBetsService filterBetsService = filterBetsService;
    private readonly IForecastService forecastService = forecastService;
    private readonly IEncryption encryption = encryption;
    private readonly AppSettings appSettings = options.Value;

    public Task<LastBetsViewModel> Handle(LastBetsQuery message, CancellationToken cancellationToken)
    {
        var viewModel = new LastBetsViewModel(
            message,
            builder,
            ticketHelper,
            LastTransIdCacheKeyByFilter(message.ViewerId),
            LatestTransIdCacheKeyByFilter(message.ViewerId));

        return Task.FromResult(viewModel);
    }

    public async Task<LastBetsDataViewModel> Handle(GetLastBetsDataQuery message, CancellationToken cancellationToken)
    {
        var param = new GetLastBetsParam(
            message.BetType,
            message.ViewerId,
            message.TransId == 0 ? 0 : message.TransId - 1,
            message.PageSize,
            appSettings.RunningMatchCacheTime,
            message.DisplayVNIP,
            message.Site);
        var tickets = await lastBetsService.GetBetList(param);
        var betListData = new BetListData(tickets, null);
        var supportScoreMapBetTypeIds = tickets.Any()
            ? await forecastService.GetBetTypeIdsSupportScoreMap()
            : [];

        var isManualLoad = !message.IsAutoLoad;
        var previousLatestTransId = isManualLoad ? 0 : message.LatestTransId;
        var newTickets = tickets.Where(t => t.TransId > previousLatestTransId).ToList();

        if (isManualLoad || newTickets.Any())
        {
            WriteLog(message.CustId, newTickets);
        }

        var latestTransId = newTickets.Any() ? newTickets.First().TransId : message.LatestTransId;

        return new LastBetsDataViewModel(latestTransId, message, betListData, builder, ticketHelper, supportScoreMapBetTypeIds);
    }

    public async Task<LastBetsNewViewModel> Handle(LastBetsNewQuery message, CancellationToken cancellationToken)
    {
        var filters = await filterBetsService.GetSavedFilters(
            message.ViewerId,
            message.SubAccId,
            message.Site,
            message.UnsavedFilter);

        var filterId = -1;
        var isShowFilterPlaceHolder = true;
        long latestTransId = 0;
        BetListData betListData = null;

        if (filters.Any())
        {
            var defaultFilter = GetDefaultFilter(filters, message.FilterName);
            if (defaultFilter != null)
            {
                filterId = defaultFilter.FilterId;
                isShowFilterPlaceHolder = false;

                var result = await GetMemberList(defaultFilter.Members, message.ViewerId, defaultFilter.FilterByMembers);
                var memberList = result.Item1;
                var memberInWatchlistIds = result.Item2;

                var param = new GetLastBetsNewParam(
                    message.ViewerId,
                    0,
                    memberList,
                    defaultFilter.SportTypes,
                    defaultFilter.Leagues,
                    defaultFilter.BetTypes,
                    defaultFilter.EventStatus,
                    defaultFilter.Stake,
                    message.LimitRowCount,
                    appSettings.RunningMatchCacheTime,
                    message.DisplayVNIP,
                    message.Site);

                (betListData, latestTransId) = await GetBetData(param, memberInWatchlistIds, latestTransId);
            }
        }

        return new LastBetsNewViewModel(
            filterId,
            filters,
            isShowFilterPlaceHolder,
            latestTransId,
            LastTransIdCacheKeyByFilter(message.ViewerId, filterId),
            LatestTransIdCacheKeyByFilter(message.ViewerId, filterId),
            message,
            betListData,
            Builder,
            TicketHelper);
    }

    public async Task<LastBetsDataNewViewModel> Handle(GetLastBetsDataNewQuery message, CancellationToken cancellationToken)
    {
        var (memberList, memberInWatchlistIds) = await GetMemberList(message.MemberList, message.ViewerId, message.FilterByMembers);
        var param = new GetLastBetsNewParam(
            message.ViewerId,
            message.TransId == 0 ? 0 : message.TransId - 1,
            memberList,
            message.SportTypeList,
            message.LeagueList,
            message.BetTypeList,
            message.EvenStatus,
            message.MinStake,
            message.LimitRowCount,
            appSettings.RunningMatchCacheTime,
            message.DisplayVNIP,
            message.Site);
        var (betListData, latestTransId) = await GetBetData(param, memberInWatchlistIds, message.LatestTransId);

        return new LastBetsDataNewViewModel(
            latestTransId,
            LastTransIdCacheKeyByFilter(message.ViewerId, message.FilterId),
            LatestTransIdCacheKeyByFilter(message.ViewerId, message.FilterId),
            message,
            betListData,
            Builder,
            TicketHelper);
    }

    private async Task<(string, IEnumerable<int>)> GetMemberList(string memberList, int agentId, byte filterByMembers)
    {
        var memberInWatchlistIds = (await filterBetsService.GetMyWatchlistMembers(agentId)).Select(member => member.CustId).ToList();
        if (filterByMembers == FilterByMembersConstants.AllWatchlistMembers)
        {
            memberList = $"{memberList},{memberInWatchlistIds.JoinWithComma()}".TrimStart(',');
        }

        return (memberList, memberInWatchlistIds);
    }

    private async Task<(BetListData, long)> GetBetData(
        GetLastBetsNewParam param,
        IEnumerable<int> memberInWatchlistIds,
        long latestTransId)
    {
        var tickets = await lastBetsService.GetBetListNew(param);
        AssignBetTypeNameAndMyWatchlistIcon(memberInWatchlistIds, tickets);
        var betListData = new BetListData(tickets, null);

        var isTransIdReset = latestTransId == 0;
        var previousLatestTransId = isTransIdReset ? 0 : latestTransId;
        var newTickets = tickets.Where(t => t.TransId > previousLatestTransId);

        if (newTickets.Any())
        {
            if (!isTransIdReset)
            {
                foreach (var ticket in newTickets)
                {
                    ticket.IsHighlight = true;
                }
            }

            WriteLog(param.CustId, newTickets);
        }

        latestTransId = newTickets.FirstOrDefault()?.TransId ?? latestTransId;

        return (betListData, latestTransId);
    }

    private void AssignBetTypeNameAndMyWatchlistIcon(IEnumerable<int> memberInWatchlistIds, IEnumerable<TicketDto> tickets)
    {
        var memberInWatchlistHashSet = new HashSet<int>(memberInWatchlistIds ?? Enumerable.Empty<int>());

        foreach (var ticket in tickets)
        {
            ticket.BetType = TicketHelper.GetBetTypeNameById(ticket.BetTypeId);
            ticket.IsInWatchlist = memberInWatchlistHashSet.Contains((int)ticket.CustId);
        }
    }

    private string LastTransIdCacheKeyByFilter(int viewerId, int filterId = 0)
        => encryption.CfsEnCode($"LastTransId_{viewerId}_{filterId}", NumberOfEncodeResult);

    private string LatestTransIdCacheKeyByFilter(int viewerId, int filterId = 0)
        => encryption.CfsEnCode($"LatestTransId_{viewerId}_{filterId}", NumberOfEncodeResult);

    private static SavedFilter GetDefaultFilter(IEnumerable<SavedFilter> filters, string filterName)
    {
        var defaultFilter = string.IsNullOrWhiteSpace(filterName)
            ? null
            : filters.FirstOrDefault(f => f.FilterName.Equals(filterName, StringComparison.OrdinalIgnoreCase));

        return defaultFilter ?? filters.FirstOrDefault(f => f.IsDefault);
    }
}