namespace AlphaPlus.Settings.MemberInfo.API.Status.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;

    public class ConfirmClosedViewModel : ConfirmStatusItemViewModelBase
    {
        public ConfirmClosedViewModel(ISearchAccountInfoRequest request, int displayOrder = 2)
            : base(request, displayOrder)
        {
        }

        private bool IsClosedUpline => Request.GetData<bool>(DataKey.SearchAccount.Upline.IsClosed);

        public override bool Disabled => base.Disabled || IsClosedUpline;

        private bool IsClosedSearchAccount => Request.GetData<bool>(DataKey.SearchAccount.IsClosed);

        public override bool Checked => IsClosedSearchAccount;

        public override bool ReloadCustomerInfo => true;

        public override bool ReloadCustomerList => true;

        public override bool ShowInCustomerList => true;

        internal override string SuffixMessage
        {
            get
            {
                if (Request.SearchAccount.IsMember && Checked)
                {
                    return "InCaseOpenedMemberMessage";
                }

                if (!Request.SearchAccount.IsMember && !Checked)
                {
                    return "InCaseClosedDownlinesMessage";
                }

                return "InCaseChangeClosedStatusMessage";
            }
        }

        internal override string StatusName => DataKey.StatusMapping.Closed;

        internal override bool StatusValue => !Checked;

        public string Tooltip 
            => IsClosedUpline && !Request.GetData<bool>(DataKey.SearchAccount.IsDirectClosed)
            ? Request.GetResource("IndirectlyClosedByUpline")
            : string.Empty;

        public override bool IsPopup => IsClosedSearchAccount && SearchAccount.IsNotMember;

        public override string PopupHeaderText => IsPopup ? Request.GetResource("OpenCustomerPopupHeaderText") : base.PopupHeaderText;

        public override string Url => IsPopup
            ? $"{AccountInfoUrl}OpenDownlines/EditSingle?SearchAccountId={SearchAccount.Id}&SearchAccountName={SearchAccount.Username}"
            : base.Url;
    }
}