using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingQuery : BaseQuery, IRequest<BetChoiceMatchingViewModel>
{
    public DateTime EventDate { get; set; } = SystemTime.Today;

    public int BetCount { get; set; } = 2;
}