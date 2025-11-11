using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class NormalResultDetailsQuery : NormalResultQuery, IRequest<NormalDetailViewModel>
{
    public int MatchId { get; set; }

    public long BetId { get; set; }

    public string Race { get; set; }

    public long RefNum { get; set; }

    public bool IsViewResultFromBetList { get; set; }

    public int Hdp2 { get; set; }

    public string DisplayBetTypeId { get; set; }
}