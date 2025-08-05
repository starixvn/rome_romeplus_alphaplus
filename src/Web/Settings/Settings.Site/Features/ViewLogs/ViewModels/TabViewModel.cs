using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels
{
    public class TabViewModel
    {
        public TabViewModel(string selectedTabName)
        {
            SelectedTabName = selectedTabName;
        }

        public TabViewModel(string selectedTabName, bool isOfferedCashService, byte level)
        {
            SelectedTabName = selectedTabName;
            IsOfferedCashService = isOfferedCashService;
            Level = level;
        }

        public string SelectedTabName { get; }

        public bool IsOfferedCashService { get; set; }

        public byte Level { get; set; }

        public bool IsAgent => Level == UserLevel.Agent.Value;
    }
}