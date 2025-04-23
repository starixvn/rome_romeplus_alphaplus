using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public class ConfirmUnlockAccessViewModel : ConfirmStatusItemViewModelBase
    {
        public ConfirmUnlockAccessViewModel(ISearchAccountInfoRequest request, int displayOrder = 3)
            : base(request, displayOrder)
        {
        }

        public override bool Disabled => base.Disabled || !Checked;

        public override bool Checked => Request.GetData<byte>(DataKey.SearchAccount.IsLockedAccess) != DataKey.LockedAccess.None;

        public override bool ReloadCustomerInfo => true;

        public override bool ReloadCustomerList => true;

        public override bool ShowInCustomerList => false;

        internal override string SuffixMessage
        {
            get
            {
                var locked = Request.GetData<byte>(DataKey.SearchAccount.IsLockedAccess);

                if (locked == DataKey.LockedAccess.FalseOTPSC)
                {
                    return "InCaseFalseOTPSC";
                }

                if (locked == DataKey.LockedAccess.FalsePassword)
                {
                    return "InCaseFalsePassword";
                }

                return "InCaseNone";
            }
        }

        internal override string StatusName => DataKey.StatusMapping.Unlock;

        internal override bool StatusValue => !Checked;
    }
}
