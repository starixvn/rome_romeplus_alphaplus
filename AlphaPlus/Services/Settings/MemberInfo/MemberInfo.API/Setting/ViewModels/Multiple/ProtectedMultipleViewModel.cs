namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;

    public class ProtectedMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        private new const int DisplayOrder = 1003;

        public ProtectedMultipleViewModel(ISearchAccountInfoRequest request) : base(request, DisplayOrder)
        {
        }

        public override string Text => Request.GetResource("EditMultipleProtectedStatusText");

        protected override string BaseAddress
            => $"{AccountInfoUrl}ProtectedStatus/EditMultiple";

        public override string Url => BaseAddress;

        public override Dictionary<string, string> QueryStringParamaters
           => new Dictionary<string, string>
           {
               ["CustIds"] = Request.GetData<string>("customerIds")
           };

        public override bool Visible
            => base.Visible
            && Request.SearchAccount.IsNotMember
            && Request.GetData<bool>(DataKey.SearchAccount.IsOfferProtectedMode);
    }
}
