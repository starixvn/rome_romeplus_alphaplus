using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public class ConfirmSuspendedViewModel : ConfirmStatusItemViewModelBase
    {
        public ConfirmSuspendedViewModel(ISearchAccountInfoRequest request, int displayOrder = 1)
         : base(request, displayOrder)
        {
        }

        private bool IsSuspendedUpline => Request.GetData<bool>(DataKey.SearchAccount.Upline.IsSuspended);

        private bool IsSuspendedSearchAccount => Request.GetData<bool>(DataKey.SearchAccount.IsSuspended);

        public override bool Disabled => base.Disabled || IsSuspendedUpline ||
            Request.GetData<bool>(DataKey.SearchAccount.Upline.IsClosed);

        public override bool Checked => IsSuspendedSearchAccount;

        public override bool ReloadCustomerInfo => true;

        public override bool ReloadCustomerList => true;

        public override bool ShowInCustomerList => true;

        internal override string SuffixMessage
        {
            get
            {
                if (Request.SearchAccount.IsMember)
                {
                    return Checked ? "InCaseUnsuspendedMemberMessage" : "InCaseSuspendedMemberMessage";
                }

                return Checked ? "InCaseUnsuspendedMasterOrAgentMessage" : "InCaseSuspendedMasterOrAgentMessage";
            }
        }

        internal override string StatusName => DataKey.StatusMapping.Suspended;

        internal override bool StatusValue => !Checked;

        public string Tooltip =>
            IsSuspendedUpline && !Request.GetData<bool>(DataKey.SearchAccount.IsDirectSuspend)
            ? Request.GetResource("SuspendedByUplineTooltip") : string.Empty;

        public override bool IsPopup => IsSuspendedSearchAccount && SearchAccount.IsNotMember;

        public override string PopupHeaderText => IsPopup ? Request.GetResource("UnuspendStatusPopupHeaderText") : base.PopupHeaderText;

        public override string Url => IsPopup ?
            MicroSites.AccountInfoUrl($"~/UnsuspendDownlines/EditSingle?CustId={SearchAccount.Id}&SearchAccountName={SearchAccount.Username}") :
            base.Url;
    }
}