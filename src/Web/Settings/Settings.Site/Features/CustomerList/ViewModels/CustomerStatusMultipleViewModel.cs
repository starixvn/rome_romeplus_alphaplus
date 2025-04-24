using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    public class CustomerStatusMultipleViewModel
    {
        public CustomerStatusMultipleViewModel(
            IEnumerable<IStatusItemViewModel> status,
            IEnumerable<ISettingItemMultipleViewModel> settings)
        {
            Status = status;
            Settings = settings;
        }

        public IEnumerable<IStatusItemViewModel> Status { get; }

        public IEnumerable<ISettingItemMultipleViewModel> Settings { get; }
    }
}