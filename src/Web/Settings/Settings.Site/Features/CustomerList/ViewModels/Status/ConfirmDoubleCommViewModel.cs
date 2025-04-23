using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public class ConfirmDoubleCommViewModel : ConfirmStatusItemViewModelBase
    {
        public ConfirmDoubleCommViewModel(ISearchAccountInfoRequest request, int displayOrder = 7)
           : base(request, displayOrder)
        {
        }

        public override bool Disabled
            => base.Disabled
                || !Request.GetData<bool>(DataKey.SearchAccount.Upline.IsAllowedDoubleComm)
                || Request.ActorIsNotEditingDirectDownline;

        public override bool Checked => Request.GetData<bool>(DataKey.SearchAccount.IsAllowedDoubleComm);

        public override bool ReloadCustomerList => true;

        public override bool ShowInCustomerList => true;

        internal override string SuffixMessage
        {
            get
            {
                if (Request.SearchAccount.IsMember)
                {
                    return Checked ? "InCaseDisabledMemberMessage" : "InCaseEnabledMemberMessage";
                }

                return Checked ? "InCaseDisabledMasterOrAgentMessage" : "InCaseEnableMasterOrAgentMessage";
            }
        }

        internal override string StatusName => DataKey.StatusMapping.DoubleComm;

        internal override bool StatusValue => !Checked;
    }
}