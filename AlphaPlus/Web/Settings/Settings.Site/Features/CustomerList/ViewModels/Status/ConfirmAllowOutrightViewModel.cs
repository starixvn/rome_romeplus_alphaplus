using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public class ConfirmAllowOutrightViewModel : ConfirmStatusItemViewModelBase
    {
        public ConfirmAllowOutrightViewModel(ISearchAccountInfoRequest request, int displayOrder = 5)
        : base(request, displayOrder)
        {
        }

        public override bool Disabled
            => base.Disabled || !Request.GetData<bool>(DataKey.SearchAccount.Upline.IsAllowedOutright);

        public string Tooltip => Disabled ? Request.GetResource("OutrightDisabledUpline") : string.Empty;

        public override bool Checked => Request.GetData<bool>(DataKey.SearchAccount.IsAllowedOutright);

        public override bool ShowInCustomerList => true;

        internal override string SuffixMessage => Checked ? "InCaseDisabledMessage" : "InCaseEnabledMessage";

        internal override string StatusName => DataKey.StatusMapping.DisableOutright;

        internal override bool StatusValue => Checked;
    }
}