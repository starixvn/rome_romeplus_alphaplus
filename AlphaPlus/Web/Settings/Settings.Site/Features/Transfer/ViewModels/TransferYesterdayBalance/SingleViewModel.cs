using AlphaPlus.Core.Extensions;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;

public class SingleViewModel(int custId, decimal amount)
{
    public int CustId { get; } = custId;

    public string Amount { get; } = (-amount).FormatNumber4();

}