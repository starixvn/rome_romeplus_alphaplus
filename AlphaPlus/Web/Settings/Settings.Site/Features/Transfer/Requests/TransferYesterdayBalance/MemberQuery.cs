using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class MemberQuery : TransferBaseQuery, IRequest<MemberViewModel>
{
    public MemberQuery()
    {
        ActorName = viewer.Name;
        AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
    }

    public string ActorName { get; }

    public bool AllowTotalBalanceTransfer { get; }

    public override bool IsValid() => ActorLevel.IsAgentLevel() && base.IsValid();
}