using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.Parlay;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Utils;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;

public class ComboParlayDetailViewModel
{
    private readonly IDictionary<string, int> statusOrder = new Dictionary<string, int>
    {
        [string.Empty] = 0,
        [BetStatus.Won] = 1,
        [BetStatus.Lose] = 2,
        [BetStatus.Draw] = 3,
        [BetStatus.Void] = 4,
        [BetStatus.Reject] = 5,
        [BetStatus.Refund] = 6
    };

    public ComboParlayDetailViewModel(
        ComboParlayDetailQuery message,
        ComboParlayDetailData comboParlayDetailData,
        ITicketHelper ticketHelper,
        bool isMobile,
        bool isExportingExcel = false)
    {
        IsDataTooHuge = comboParlayDetailData.ComboParlayDetails.Count() > 5000;
        Message = message;

        Title = message.BetTeam == "1" ? Resources.lblMixParlay : Resources.lblSystemParlay;

        var allowInitializingData = isExportingExcel || !IsDataTooHuge;

        if (allowInitializingData)
        {
            ComboParlayDetailWrappers = new List<ComboParlayDetailWrapper>(comboParlayDetailData.ComboParlayDetails
                    .OrderBy(i => statusOrder[i.Status?.ToLowerInvariant() ?? string.Empty])
                    .Select(d => new ComboParlayDetailWrapper(d, comboParlayDetailData.MatchDetails, ticketHelper)));
            ComboParlayDetailWrapperGroups = ComboParlayDetailWrappers.GroupBy(w => w.WinLostDate).OrderBy(i => i.Key);

            OutStanding = ConvertByBetType.Stake(comboParlayDetailData.Outstanding);
            IsParlayBoost = comboParlayDetailData.IsParlayBoost;
            TotalWinLost = ConvertByBetType.Winloss(comboParlayDetailData.ComboParlayDetails.Sum(d => d.WinLost + d.PlayerComm));
            IsRunningTicket = !comboParlayDetailData.ComboParlayDetails.Any();
        }

        IsMobile = isMobile;
    }

    public ComboParlayDetailQuery Message { get; }

    public bool IsMobile { get; }

    public IEnumerable<ComboParlayDetailWrapper> ComboParlayDetailWrappers { get; }

    public IEnumerable<IGrouping<string, ComboParlayDetailWrapper>> ComboParlayDetailWrapperGroups { get; }

    public string TotalWinLost { get; }

    public string OutStanding { get; }

    public bool IsParlayBoost { get; }

    public string Title { get; }

    public bool IsDataTooHuge { get; }

    public bool IsRunningTicket { get; }

    public string HugeDataLabel => string.Format(Resources.CombinationResultTooHuge, BuildComboParlayDetailExcelPath(Message));

    public string SubTotalWinLost(IEnumerable<ComboParlayDetailWrapper> group)
        => ConvertByBetType.Winloss(group.Sum(item => item.ComboParlayDetail.WinLost + item.ComboParlayDetail.PlayerComm));

    private static string BuildComboParlayDetailExcelPath(ComboParlayDetailQuery message)
    {
        // var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        // return urlHelper.Action("ComboParlayDetailExcel", "Parlay", query);
        return $"ComboParlayDetailExcel?refno={message.RefNo}&winLostDate={message.WinLostDate.ToShortDateWithSlash()}&betTeam={message.BetTeam}";
    }
}