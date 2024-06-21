using AlphaPlus.BetLists.Shared.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class DeleteFiltersCommand : BetListBaseQuery, IRequest<IEnumerable<Result>>
{
    public DeleteFiltersCommand() : base()
    {
        LanguageKey = Viewer.LangKey;
        IP = Viewer.IP;
    }

    public string Ids { get; set; }

    public string LanguageKey { get; }

    public string IP { get; }

    public bool IsValid() => !string.IsNullOrWhiteSpace(Ids);
}