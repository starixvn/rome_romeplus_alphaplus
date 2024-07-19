using AlphaPlus.Core.Extensions;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.MultipleTransfer;

public class SingleTransferViewModel(
    SingleTransferQuery message,
    decimal amount)
{
    public int CustId { get; } = message.CustId;

    public string Amount { get; } = (-amount).FormatNumber4();
}