using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class MemberCommand : TransferBaseQuery, IRequest<Result>
{
    public MemberCommand(IUser viewer)
    {
        IsInternal = viewer.IsInternal;
        LangKey = viewer.LanguageKey;
        IP = viewer.IP;
        SubAccId = viewer.SubAccId;
        ActorName = SubAccId > 0 ? viewer.SubAccName : viewer.Name;
    }

    public decimal Amount { get; set; }

    public bool IsInternal { get; }

    public string LangKey { get; }

    public string IP { get; }

    public int SubAccId { get; }

    public string ActorName { get; }

    public override bool IsValid() => ActorLevel.IsAgentLevel() && base.IsValid();
}