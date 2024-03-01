namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using System.Collections.Generic;

    public class SuspendedMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        private new const int DisplayOrder = 1001;
        private readonly ISearchAccountInfoRequest request;

        public SuspendedMultipleViewModel(ISearchAccountInfoRequest request) : base(request, DisplayOrder)
        {
            this.request = request;
        }

        public override string Text => Request.GetResource("EditMultipleSuspendedStatusText");

        protected override string BaseAddress
            => $"{AccountInfoUrl}UnsuspendDownlines/EditMultiple";

        public override string Url => BaseAddress;

        public override Dictionary<string, string> QueryStringParamaters
            => new Dictionary<string, string>
            {
                ["CustIds"] = Request.GetData<string>("customerIds"),
                ["RoleId"] = SearchAccount.CusomerLevelValue.ToString()
            };

        public override bool Visible => !(request.Actor.IsSuper && request.SearchAccount.IsAgent);
    }
}