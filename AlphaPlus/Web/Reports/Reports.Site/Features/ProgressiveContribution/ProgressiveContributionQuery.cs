using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionQuery : DefaultDateRangeQuery, IRequest<ProgressiveContributionViewModel>
{
    public string UserSelectedProductIds { get; set; }

    public bool IsFilterVisible { get; set; } = true;

    public bool IsStackMode { get; set; }

    public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);

    public override bool IsValid() => IsEnabledJackpot && base.IsValid();
}
