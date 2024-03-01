namespace AlphaPlus.Settings.MemberInfo.API.Status.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;

    public class ConfirmAllowAutoMasterPTViewModel : ConfirmStatusItemViewModelBase
    {
        public ConfirmAllowAutoMasterPTViewModel(ISearchAccountInfoRequest request, int displayOrder = 6)
          : base(request, displayOrder)
        {
        }

        public override bool Visible => Actor.IsSuper && SearchAccount.IsMaster;

        public override bool Checked => Request.GetData<bool>(DataKey.SearchAccount.IsAllowedMasterAutoPT);

        public override bool ShowInCustomerList => true;

        internal override string SuffixMessage => Checked ? "InCaseDisabledMessage" : "InCaseEnabledMessage";

        internal override string StatusName => DataKey.StatusMapping.DisableAutoMasterPT;

        internal override bool StatusValue => Checked;
    }
}