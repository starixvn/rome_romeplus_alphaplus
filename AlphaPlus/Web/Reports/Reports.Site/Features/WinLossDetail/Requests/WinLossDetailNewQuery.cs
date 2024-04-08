using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewQuery : ProductDetailDateRangeQuery, IRequest<WinLossDetailNewViewModel>
{
    public WinLossDetailNewQuery() : base()
        => IsProtectedMode = Viewer.IsProtectedMode;

    public bool IsProtectedMode { get; }

    public string Username { get; set; }
}