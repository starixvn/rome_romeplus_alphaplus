namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using AlphaPlus.Settings.MemberInfo.API.Status.ViewModels.Base;
    using System.Collections.Generic;

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