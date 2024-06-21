using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class GetLastBetsDataQuery : LastBetsQuery, IRequest<LastBetsDataViewModel>
{
    public long TransId { get; set; }

    public bool IsAutoLoad { get; set; }

    public long LatestTransId { get; set; }
}