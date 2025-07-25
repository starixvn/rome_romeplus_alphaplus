using AlphaPlus.Settings.ViewLogs.API.Params;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels.CashService
{
    public class IndexViewModel : LogViewModel
    {
        public IndexViewModel(LogParam logParam, int totalRecords) : base(logParam, totalRecords)
        {
        }

        public IEnumerable<LogActionTypeViewModel> LogActionTypes { get; set; }
    }
}