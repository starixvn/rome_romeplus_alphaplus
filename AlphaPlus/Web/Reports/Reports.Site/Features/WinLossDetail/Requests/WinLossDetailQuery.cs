using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailQuery : ProductDetailDateRangeQuery, IRequest<WinLossDetailViewModel>
{
    public WinLossDetailQuery()
        => IsProtectedMode = Viewer.IsProtectedMode;

    public bool IsProtectedMode { get; }
}