using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;

public class MultipleMemberCommand : TransferBaseQuery, IRequest<Result>
{
    public MultipleMemberCommand() : base()
    {
        AllowTotalBalanceTransfer = viewer.AllowTotalBalanceTransfer;
        IsInternal = viewer.IsInternal;
        LangKey = viewer.LanguageKey;
        IP = viewer.IP;
        SubAccId = viewer.SubAccId;
    }

    public string CustIds { get; set; }

    public string Username { get; set; }

    public string Status { get; set; }

    public byte AutoTransfer { get; set; }

    public bool AllYesterdayBalance { get; set; }

    public bool AllowTotalBalanceTransfer { get; }

    public bool IsInternal { get; }

    public string LangKey { get; }

    public string IP { get; }

    public int SubAccId { get; }
}
