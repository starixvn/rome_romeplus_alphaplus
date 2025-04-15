using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class SingleQuery : TransferBaseQuery, IRequest<SingleViewModel>
{
    public SingleQuery()
    {
        AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
    }

    public bool AllowTotalBalanceTransfer { get; }

    public override bool IsValid() => ActorLevel.IsMasterOrSuperLevel() && base.IsValid();
}