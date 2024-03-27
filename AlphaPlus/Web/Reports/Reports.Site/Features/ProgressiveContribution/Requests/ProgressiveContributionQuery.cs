using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionQuery : DefaultDateRangeQuery, IRequest<ProgressiveContributionViewModel>
{
    private const string GameEnableJackpot = "GameEnable_Jackpot";

    public ProgressiveContributionQuery() : base()
    {
        EnableJackpotProductIds = Viewer.GetProperty<IList<int>>(GameEnableJackpot) ?? [];
        IsEnabledJackpot = EnableJackpotProductIds.Any();
    }

    public string UserSelectedProductIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);

    public IList<int> EnableJackpotProductIds { get; }

    public bool IsEnabledJackpot { get; }

    public override bool IsValid() => IsEnabledJackpot && base.IsValid();
}
