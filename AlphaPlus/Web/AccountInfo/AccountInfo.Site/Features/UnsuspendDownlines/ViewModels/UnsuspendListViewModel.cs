using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Models;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels
{
    public class UnsuspendListViewModel
    {
        public UnsuspendListViewModel(
           int searchAccountId,
           string searchAccountName,
           UnsuspendViewListModel data)
        {
            SearchAccountId = searchAccountId;
            SearchAccountName = searchAccountName;
            Data = data;
        }

        public int SearchAccountId { get; private set; }

        public string SearchAccountName { get; private set; }

        public UnsuspendViewListModel Data { get; private set; }
    }
}