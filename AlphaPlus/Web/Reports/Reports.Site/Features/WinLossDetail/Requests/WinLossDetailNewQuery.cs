using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewQuery : WinLossDetailGetDataQuery, IRequest<WinLossDetailNewViewModel>
{
    public string VisibleColumns { get; set; }
}