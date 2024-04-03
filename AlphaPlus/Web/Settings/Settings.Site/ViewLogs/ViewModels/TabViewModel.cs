namespace AlphaPlus.Settings.Site.ViewLogs.ViewModels
{
    public class TabViewModel
    {
        public TabViewModel(string selectedTabName)
        {
            SelectedTabName = selectedTabName;
        }

        public string SelectedTabName { get; }
    }
}