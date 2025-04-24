using AlphaPlus.BetLists.Shared.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class UpdateFilterCommand : AddUnsavedFilterCommand, IRequest<Result>
{
    public int FilterId { get; set; }

    public override bool IsValid() => FilterId > 0 && base.IsValid();
}