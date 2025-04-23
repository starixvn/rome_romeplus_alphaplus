using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.CashBalanceTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer;

public class HintTransferQuery : TransferBaseQuery, IRequest<HintTransferViewModel>
{
    public HintTransferQuery()
    {
        Language = viewer.Language;
        Currency = viewer.Currency;
    }

    public string Language { get; }

    public string Currency { get; }
}