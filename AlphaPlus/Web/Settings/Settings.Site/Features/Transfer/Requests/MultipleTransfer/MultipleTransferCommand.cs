using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Constants;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;

public class MultipleTransferCommand : TransferBaseQuery, IRequest<Result>
{
    public MultipleTransferCommand() : base()
        => AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;

    public string CustIds { get; set; }

    public string Username { get; set; }

    public string Status { get; set; } = StatusConstant.Open;

    public byte AutoTransfer { get; set; } = AutoTransferConstant.All;

    public bool AllYesterdayBalance { get; set; }

    public bool AllowTotalBalanceTransfer { get; }

    public bool IsInternal => viewer.IsInternal;

    public string LangKey => viewer.LanguageKey;

    public string IP => viewer.IP;

    public int SubAccId => viewer.SubAccId;
}
