namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class DownlineListViewModel
    {
        public DownlineListViewModel(DownlineListModel model, IEnumerable<TotalDownlinesSummary> totalDownlines)
        {
            DownlineList = model.Downlines;
            TotalDownlines = totalDownlines.OrderByDescending(item => item.RoleId);
            TotalPages = model.TotalPages;
        }

        public IEnumerable<DownlineDto> DownlineList { get; }

        public IEnumerable<TotalDownlinesSummary> TotalDownlines { get; }

        public int TotalPages { get; }
    }
}