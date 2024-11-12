using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailQuery : WinLossDetailGetDataQuery, IRequest<WinLossDetailViewModel>
{
    public string VisibleColumns { get; set; }
}