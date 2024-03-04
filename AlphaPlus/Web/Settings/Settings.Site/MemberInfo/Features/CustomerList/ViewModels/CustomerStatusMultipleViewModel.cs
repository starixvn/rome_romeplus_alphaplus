namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using AlphaPlus.Settings.MemberInfo.API.Status.ViewModels.Base;
    using System.Collections.Generic;

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