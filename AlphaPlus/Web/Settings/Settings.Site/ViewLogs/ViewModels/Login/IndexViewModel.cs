namespace AlphaPlus.Settings.Site.ViewLogs.ViewModels.Login
{
    using AlphaPlus.Settings.ViewLogs.API.Params;

    public class IndexViewModel : LogViewModel
    {
        public IndexViewModel(LogParam logParam, int totalRecords) : base(logParam, totalRecords)
        {
        }

        public bool IsAgent { get; set; }

        public string PreviousUsername { get; set; }
    }
}