using AlphaPlus.Settings.Site.Features.Transfer.Constants;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferList;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferList;

public class TransferListQuery : TransferBaseQuery, IRequest<TransferListViewModel>
{
    public TransferListQuery()
    {
        AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
        Currency = viewer.Currency;
    }

    public bool? ADBT { get; set; }

    public bool? IsPostBack { get; set; }

    public string Username { get; set; }

    public string Status { get; set; } = StatusConstant.Open;

    public byte AutoTransfer { get; set; } = AutoTransferConstant.All;

    public byte LevelType { get; set; }

    public bool AllYesterdayBalance { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 500;

    public bool AllowTotalBalanceTransfer { get; }

    public string Currency { get; }
}
