using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class Result3rdQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
    public int SportTypeId { get; set; }

    public int RefNo { get; set; }
}