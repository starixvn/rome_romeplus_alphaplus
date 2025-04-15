namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class DownlineListViewModel
    {
        public DownlineListViewModel(DownlineListModel model)
        {
            DownlineList = model.Downlines;
            TotalPages = model.TotalPages;
        }

        public IEnumerable<DownlineDto> DownlineList { get; }


        public int TotalPages { get; }
    }
}