using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class MemberQuery : TransferBaseQuery, IRequest<MemberViewModel>
{
    public MemberQuery() : base()
    {
        AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
        ActorName = viewer.Name;
    }

    public bool AllowTotalBalanceTransfer { get; }

    public string ActorName { get; }
}