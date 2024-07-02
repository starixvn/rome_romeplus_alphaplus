using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailGetWeeksQuery : WinLossDetailNewQuery, IRequest<IEnumerable<WinLossDetailWeekViewModel>>
{
}