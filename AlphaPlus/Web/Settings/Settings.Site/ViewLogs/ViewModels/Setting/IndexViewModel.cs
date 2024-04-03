namespace AlphaPlus.Settings.Site.ViewLogs.ViewModels.Setting
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.ViewLogs.API.Params;

    public class IndexViewModel : LogViewModel
    {
        public IndexViewModel(LogParam logParam, int totalRecords) : base(logParam, totalRecords)
        {
        }

        public IEnumerable<LogActionTypeViewModel> LogActionTypes { get; set; }
    }
}