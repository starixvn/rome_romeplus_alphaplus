namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using System.Collections.Generic;

    public class DoubleCommMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        private new const int DisplayOrder = 1000;

        public DoubleCommMultipleViewModel(ISearchAccountInfoRequest request) : base(request, DisplayOrder)
        {
        }

        public override string Text => Request.GetResource("EditMultipleDoubleCommText");

        protected override string BaseAddress
            => $"{MemberInfoUrl}CustomerList/MultipleDoubleComm";

        public override Dictionary<string, string> QueryStringParamaters
              => new Dictionary<string, string>
              {
                  ["SearchAccountIds"] = Request.GetData<string>("customerIds"),
                  ["SearchAccountRoleId"] = SearchAccount.CusomerLevelValue.ToString()
              };
    }
}