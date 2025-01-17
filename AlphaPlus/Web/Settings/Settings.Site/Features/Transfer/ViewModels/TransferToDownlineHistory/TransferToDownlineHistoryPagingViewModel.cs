using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;

public class TransferToDownlineHistoryPagingViewModel(
    IEnumerable<TransferToDownlineHistoryModel> transferToDownlines,
    int totalRecords)
{
    public IEnumerable<TransferToDownlineHistoryModel> TransferToDownlines { get; } = transferToDownlines;

    public int TotalRecords { get; } = totalRecords;
}