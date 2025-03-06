using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class MemberCommand : TransferBaseQuery, IRequest<Result>
{
    public MemberCommand()
    {
        IsInternal = viewer.IsInternal;
        LangKey = viewer.LanguageKey;
        IP = viewer.IP;
        SubAccId = viewer.SubAccId;
        ActorName = viewer.Name;

        if (SubAccId > 0)
        {
            AdminId = SubAccId;
            AdminName = viewer.SubAccName;
        }
        else
        {
            AdminId = viewer.Id;
            AdminName = viewer.Name;
        }
    }

    public decimal Amount { get; set; }

    public bool IsInternal { get; }

    public string LangKey { get; }

    public string IP { get; }

    public int SubAccId { get; }

    public string ActorName { get; }

    public int AdminId { get; }

    public string AdminName { get; }

    public override bool IsValid()
        => !IsInternal
        && ActorLevel.IsAgentLevel()
        && ViewerId != CustId
        && base.IsValid();
}