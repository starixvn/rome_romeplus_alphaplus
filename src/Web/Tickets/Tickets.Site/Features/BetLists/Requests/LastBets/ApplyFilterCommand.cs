using AlphaPlus.BetLists.Shared.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class ApplyFilterCommand : AddFilterCommand, IRequest<Result>
{
}
