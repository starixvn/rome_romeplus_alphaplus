using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.Common.Extensions;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;

public class WinLossQuery : BetListBaseQuery, IRequest<WinLossViewModel>
{
    protected const int NegativeThree = -3;

    public WinLossQuery()
    {
        LimitedDateToShowInfo = Viewer.LimitedDateToShowInfo;
    }

    public bool IsGroupHeadline { get; set; }

    public virtual DateTime MinDate
        => IsGroupHeadline
        ? SystemTime.Today.AddMonths(NegativeThree).FirstDayOfMonth()
        : DateTime.MinValue;

    public DateTime LimitedDateToShowInfo { get; }

    public bool IsExcel { get; protected set; }
}
