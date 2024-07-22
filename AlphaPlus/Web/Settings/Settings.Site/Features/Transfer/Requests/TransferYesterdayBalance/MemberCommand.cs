using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class MemberCommand : TransferBaseQuery, IRequest<Result>
{
    public MemberCommand() : base()
    {
        ActorName = viewer.Name;
        IsInternal = viewer.IsInternal;
        LangKey = viewer.LanguageKey;
        IP = viewer.IP;
        SubAccId = viewer.SubAccId;
        SubAccName = viewer.SubAccName;
    }

    public decimal Amount { get; set; }

    public string ActorName { get; }

    public bool IsInternal { get; }

    public string LangKey { get; }

    public string IP { get; }

    public int SubAccId { get; }

    public string SubAccName { get; }
}