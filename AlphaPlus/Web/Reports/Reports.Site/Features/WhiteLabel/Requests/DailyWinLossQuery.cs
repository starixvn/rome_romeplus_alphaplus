using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class DailyWinLossQuery : DailyWinLossGetDataQuery, IRequest<DailyWinLossViewModel>
{
}
