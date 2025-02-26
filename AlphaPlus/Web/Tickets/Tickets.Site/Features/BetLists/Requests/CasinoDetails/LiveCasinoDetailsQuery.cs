using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CasinoDetails;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;

public class LiveCasinoDetailsQuery : CasinoDetailsQuery, IRequest<CasinoDetailsViewModel>;