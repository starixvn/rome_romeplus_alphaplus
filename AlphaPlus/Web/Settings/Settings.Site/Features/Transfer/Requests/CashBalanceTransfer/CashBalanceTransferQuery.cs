using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.CashBalanceTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer;

public class CashBalanceTransferQuery : TransferBaseQuery, IRequest<CashBalanceTransferViewModel>
{
    public bool IsDirectMember { get; set; }
}