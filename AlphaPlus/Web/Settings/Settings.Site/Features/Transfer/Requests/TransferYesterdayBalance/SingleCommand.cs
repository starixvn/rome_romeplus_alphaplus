using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class SingleCommand : TransferBaseQuery, IRequest<Result>
{
    public SingleCommand()
    {
        IsInternal = viewer.IsInternal;
        LangKey = viewer.LanguageKey;
        IP = viewer.IP;
        SubAccId = viewer.SubAccId;
    }

    public decimal Amount { get; set; }

    public bool IsInternal { get; }

    public string LangKey { get; }

    public string IP { get; }

    public int SubAccId { get; }

    public override bool IsValid() => ActorLevel.IsMasterOrSuperLevel() && base.IsValid();
}