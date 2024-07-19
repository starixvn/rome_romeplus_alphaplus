using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;

public class SingleTransferCommand : TransferBaseQuery, IRequest<Result>
{
    public decimal Amount { get; set; }

    public bool IsInternal => viewer.IsInternal;

    public string LangKey => viewer.LanguageKey;

    public string IP => viewer.IP;

    public int SubAccId => viewer.SubAccId;
}