using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    public class CustomerStatusViewModel
    {
        public CustomerStatusViewModel(
            IEnumerable<IStatusItemViewModel> status,
            IEnumerable<ISettingItemViewModel> settings)
        {
            Status = status;
            Settings = settings;
        }

        public IEnumerable<IStatusItemViewModel> Status { get; }

        public IEnumerable<ISettingItemViewModel> Settings { get; }
    }
}