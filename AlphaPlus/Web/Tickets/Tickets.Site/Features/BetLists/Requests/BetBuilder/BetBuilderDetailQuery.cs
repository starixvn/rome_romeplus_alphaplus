using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetBuilder;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilder;

public class BetBuilderDetailQuery : IRequest<BetBuilderDetailViewModel>
{
    public long TransactionId { get; set; }

    public int CustomerId { get; set; }

    public string Status { get; set; }
}
