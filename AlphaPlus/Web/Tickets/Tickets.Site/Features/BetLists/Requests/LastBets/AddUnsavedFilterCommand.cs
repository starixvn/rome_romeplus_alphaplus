using AlphaPlus.BetLists.Shared.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class AddUnsavedFilterCommand : BetListBaseQuery, IRequest<Result>
{
    public AddUnsavedFilterCommand() : base()
    {
        LanguageKey = Viewer.LangKey;
        IP = Viewer.IP;
    }

    public string FilterName { get; set; }

    public bool IsDefault { get; set; }

    public string LanguageKey { get; }

    public string IP { get; }

    public virtual bool IsValid() => !string.IsNullOrWhiteSpace(FilterName);
}