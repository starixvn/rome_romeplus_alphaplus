namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory
{
    using AlphaPlus.Settings.Transfer.API.Models.TransferToDownlineHistory;
    using System.Collections.Generic;

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