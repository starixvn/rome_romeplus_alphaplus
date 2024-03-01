namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using System.Collections.Generic;

    public class ClosedMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        private new const int DisplayOrder = 1002;
        private readonly ISearchAccountInfoRequest request;

        public ClosedMultipleViewModel(ISearchAccountInfoRequest request) : base(request, DisplayOrder)
        {
            this.request = request;
        }

        public override string Text => Request.GetResource("EditMultipleClosedStatusText");

        public string Title => Request.GetResource("UpdateMultipleClosedStatusText");

        protected override string BaseAddress
            => $"{AccountInfoUrl}OpenDownlines/EditMultiple";

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