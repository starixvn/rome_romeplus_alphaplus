using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;

public class WinLossWeekSelectionQuery : WinLossQuery, IRequest<WinLossWeekSelectionViewModel>
{
    public override DateTime MinDate
        => SystemTime.Today.AddYears(NegativeThree)
        .FirstDayOfYear()
        .GetGreaterDate(LimitedDateToShowInfo);

    public string BreadcrumbsJson { get; set; }

    public virtual bool IsValid() => MinDate <= FromDate && FromDate <= ToDate;
}