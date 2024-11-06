using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory
{
    public class TransferToDownlineHistoryPagingViewModel
    {
        public TransferToDownlineHistoryPagingViewModel(
            IEnumerable<TransferToDownlineHistoryModel> transferToDownlines,
            int totalRecords)
        {
            TransferToDownlines = transferToDownlines;
            TotalRecords = totalRecords;
        }

        public IEnumerable<TransferToDownlineHistoryModel> TransferToDownlines { get; }

        public int TotalRecords { get; }
    }
}