using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossQuery : DefaultDateRangeQuery, IRequest<WinLossViewModel>
{
    public string UserSelectedProductIds { get; set; } = "*";

    public string UserSelectedResourceIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public string SortPropertyName { get; set; } = nameof(WinLossModel.CustName);

    public byte DescendingSort { get; set; }

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;
}