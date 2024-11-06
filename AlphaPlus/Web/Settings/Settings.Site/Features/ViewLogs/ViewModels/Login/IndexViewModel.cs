using AlphaPlus.Settings.ViewLogs.API.Params;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels.Login
{
    public class IndexViewModel : LogViewModel
    {
        public IndexViewModel(LogParam logParam, int totalRecords) : base(logParam, totalRecords)
        {
        }

        public bool IsAgent { get; set; }

        public string PreviousUsername { get; set; }
    }
}