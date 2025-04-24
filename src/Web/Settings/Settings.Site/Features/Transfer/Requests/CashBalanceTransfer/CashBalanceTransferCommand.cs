using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer;

public class CashBalanceTransferCommand : TransferBaseQuery, IRequest<Result>
{
    public CashBalanceTransferCommand()
    {
        SubAccId = viewer.SubAccId;
        IP = viewer.IP;
        LanguageKey = viewer.LanguageKey;
        UplineName = SubAccId > 0 ? viewer.SubAccName : viewer.Name;
        IsInternal = viewer.IsInternal;
    }

    public decimal Amount { get; set; }

    public bool IsDirectMember { get; set; }

    public string UplineName { get; }

    public int SubAccId { get; }

    public string IP { get; }

    public string LanguageKey { get; }

    public bool IsInternal { get; }

    public override bool IsValid() => Amount != 0 && ViewerId > 0 && base.IsValid();
}