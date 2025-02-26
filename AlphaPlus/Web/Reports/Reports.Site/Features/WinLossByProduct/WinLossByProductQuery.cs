using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossByProduct;

public class WinLossByProductQuery : BalanceUpDateRangeQuery, IRequest<WinLossByProductViewModel>
{
    public string UserSelectedProductIds { get; set; } = "*";

    public string UserSelectedResourceIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;
}