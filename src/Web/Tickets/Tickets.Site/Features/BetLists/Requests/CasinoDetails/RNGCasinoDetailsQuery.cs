using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CasinoDetails;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
public class RNGCasinoDetailsQuery : CasinoDetailsQuery, IRequest<CasinoDetailsViewModel>;