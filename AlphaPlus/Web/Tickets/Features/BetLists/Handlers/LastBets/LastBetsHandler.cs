using AlphaPlus.BetLists.LastBets;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Utils;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.LastBets;

public class LastBetsHandler(
    ILastBetsService lastBetsService,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    IForecastService forecastService,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IEncryption encryption,
    IOptions<AppSettings> options) :
        BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
        IRequestHandler<LastBetsQuery, LastBetsViewModel>,
        IRequestHandler<GetLastBetsDataQuery, LastBetsDataViewModel>
{
    private const int NumberOfEncodeResult = 6;
    private readonly ILastBetsService lastBetsService = lastBetsService;
    private readonly IForecastService forecastService = forecastService;
    private readonly IEncryption encryption = encryption;
    private readonly AppSettings appSettings = options.Value;

    public Task<LastBetsViewModel> Handle(LastBetsQuery message, CancellationToken cancellationToken)
    {
        var viewModel = new LastBetsViewModel(
            message,
            builder,
            ticketHelper,
            encryption.CfsEnCode($"LastTransId_{message.ViewerId}", NumberOfEncodeResult),
            encryption.CfsEnCode($"LatestTransId_{message.ViewerId}", NumberOfEncodeResult));

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
}