using AlphaPlus.BetLists.LastBets;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;

public class LastBetsNewViewModel(
    int filterId,
    IEnumerable<SavedFilter> filters,
    bool isShowFilterPlaceHolder,
    long latestTransId,
    string lastTransIdCacheKeyByFilter,
    string latestTransIdCacheKeyByFilter,
    LastBetsNewQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : LastBetsDataNewViewModel(
        latestTransId,
        lastTransIdCacheKeyByFilter,
        latestTransIdCacheKeyByFilter,
        message,
        betListData,
        builder,
        ticketHelper)
{
    public int FilterId { get; } = filterId;

    public IEnumerable<SavedFilter> Filters { get; } = filters;

    public bool IsShowFilterPlaceHolder { get; } = isShowFilterPlaceHolder;

    public IEnumerable<SelectListItem> MostRecentTickets { get; } =
    [
        BuildSelectListItem(100, 100 == message.LimitRowCount),
        BuildSelectListItem(300, 300 == message.LimitRowCount),
        BuildSelectListItem(500, 500 == message.LimitRowCount)
    ];

    public int[] AutoRefreshSeconds { get; } = [30, 60, 90];

    public bool AutoStart { get; } = message.AutoStart;

    public bool IsInternal { get; } = message.IsInternal;

    private static SelectListItem BuildSelectListItem(int value, bool selected)
        => new SelectListItem { Text = value.ToString(), Value = value.ToString(), Selected = selected };

    public string UserGuideLink
    {
        get
        {
            if (AppSettings.SupportedNewHandbook)
            {
                return MicroSites.FeatureUrl("~/HandbookDefault/GetBySlug?slug=last-bets");
            }

            return AppSettings.IsSiteLabel789y
                ? MicroSites.FeatureUrl("~/StaticMessage/Guide?id=186142")
                : MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=186142");
        }
    }
}