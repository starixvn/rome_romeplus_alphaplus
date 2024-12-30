using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionQuery : DefaultDateRangeQuery, IRequest<ProgressiveContributionViewModel>
{
    public string UserSelectedProductIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;

    public override bool IsValid() => EnableJackpotProductIds.Any() && base.IsValid();
}
