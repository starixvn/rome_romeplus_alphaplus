using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingQuery : BaseQuery, IRequest<BetChoiceMatchingViewModel>
{
    public DateTime EventDate { get; set; } = DateTime.Today;

    public int BetCount { get; set; } = 2;
}