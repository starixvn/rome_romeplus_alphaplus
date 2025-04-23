using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CorrectScore;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CorrectScore;

public class CorrectScoreQuery : BetListByBetTypeQuery, IRequest<CorrectScoreViewModel>
{
    public CorrectScoreQuery() => BetType = BetTypeConstants.CorrectScore;
}