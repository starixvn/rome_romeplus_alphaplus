using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SexyGamingResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
    public string Site { get; set; }
}