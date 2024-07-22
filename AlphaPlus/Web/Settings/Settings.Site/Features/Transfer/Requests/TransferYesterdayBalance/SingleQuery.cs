using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class SingleQuery : TransferBaseQuery, IRequest<SingleViewModel>
{
    public SingleQuery() : base()
        => AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;

    public bool AllowTotalBalanceTransfer { get; }
}