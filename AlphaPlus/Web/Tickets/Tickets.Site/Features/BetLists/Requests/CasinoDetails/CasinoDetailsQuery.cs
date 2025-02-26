using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CasinoDetails;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;

public class CasinoDetailsQuery : BetListBaseQuery, IRequest<CasinoDetailsViewModel>
{
    public long RefNo { get; set; }

    public byte TransType { get; set; }

    public string SportName { get; set; }

    public int ProductId { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 500;
}
