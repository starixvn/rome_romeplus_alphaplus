namespace AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels
{
    public class TabViewModel
    {
        public TabViewModel(string selectedTabName)
        {
            SelectedTabName = selectedTabName;
        }

        public TabViewModel(string selectedTabName, bool isOfferedCashService)
        {
            SelectedTabName = selectedTabName;
            IsOfferedCashService = isOfferedCashService;
        }

        public string SelectedTabName { get; }

        public bool IsOfferedCashService { get; set; }
    }
}