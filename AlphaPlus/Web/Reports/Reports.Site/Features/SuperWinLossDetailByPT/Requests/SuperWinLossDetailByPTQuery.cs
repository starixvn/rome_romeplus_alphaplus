using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTQuery : DefaultDateRangeQuery, IRequest<SuperWinLossDetailByPTViewModel>
{
    public string UserSelectedProductIds { get; set; } = "*";

    public string UserSelectedResourceIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public string SortPropertyName { get; set; } = "Turnover";

    public byte DescendingSort { get; set; } = 1;

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;

    public override bool IsValid() => ViewerLevel.IsSuperLevel() && base.IsValid();
}