using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.MultipleTransfer;
using AlphaPlus.Settings.Transfer.API.Constants;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;

public class TransferListQuery : TransferBaseQuery, IRequest<TransferListViewModel>
{
    public TransferListQuery() : base()
        => AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;

    public string Username { get; set; }

    public string Status { get; set; } = "open";

    public byte AutoTransfer { get; set; } = 2;

    public bool AllYesterdayBalance { get; set; }

    public int PageIndex { get; set; } = PagerConstant.DefaultPageIndex;

    public int PageSize { get; set; } = PagerConstant.DefaultPageSize;

    public bool AllowTotalBalanceTransfer { get; }
}
