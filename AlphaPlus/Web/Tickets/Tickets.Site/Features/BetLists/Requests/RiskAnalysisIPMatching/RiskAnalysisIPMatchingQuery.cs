using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RiskAnalysisIPMatching;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.RiskAnalysisIPMatching;

public class RiskAnalysisIPMatchingQuery : BetListBaseQuery, IRequest<RiskAnalysisIPMatchingViewModel>
{
    public string IP { get; set; }

    public DateTime Date { get; set; }
}