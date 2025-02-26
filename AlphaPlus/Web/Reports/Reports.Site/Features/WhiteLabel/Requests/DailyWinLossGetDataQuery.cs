using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class DailyWinLossGetDataQuery : DefaultDateRangeQuery, IRequest<DailyWinLossGetDataViewModel>
{
    public string WhiteLabels { get; set; }

    public string Username { get; set; }
}