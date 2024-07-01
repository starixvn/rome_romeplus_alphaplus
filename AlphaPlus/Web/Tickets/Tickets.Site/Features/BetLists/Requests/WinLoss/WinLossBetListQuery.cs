using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;

public class WinLossBetListQuery : WinLossQuery, IRequest<WinLossBetListViewModel>
{
    public override DateTime MinDate
        => SystemTime.Today.AddYears(NegativeThree)
        .FirstDayOfYear()
        .GetGreaterDate(LimitedDateToShowInfo);

    public virtual bool IsValid() => MinDate <= FromDate && FromDate <= ToDate;
}
