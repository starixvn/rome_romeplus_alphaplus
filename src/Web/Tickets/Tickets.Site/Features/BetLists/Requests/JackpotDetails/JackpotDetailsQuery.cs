using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.JackpotDetails;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.JackpotDetails;

public class JackpotDetailsQuery : BetListBaseQuery, IRequest<JackpotDetailsViewModel>
{
    public long TransId { get; set; }

    public int BetType { get; set; }

    public int ProductId { get; set; }
}
