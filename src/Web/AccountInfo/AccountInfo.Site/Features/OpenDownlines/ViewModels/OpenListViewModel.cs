using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Models;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels
{
    public class OpenListViewModel
    {
        public OpenListViewModel(
            int searchAccountId,
            string searchAccountName,
            OpenViewListModel data)
        {
            SearchAccountId = searchAccountId;
            SearchAccountName = searchAccountName;
            Data = data;
        }

        public int SearchAccountId { get; }

        public string SearchAccountName { get; }

        public OpenViewListModel Data { get; }
    }
}