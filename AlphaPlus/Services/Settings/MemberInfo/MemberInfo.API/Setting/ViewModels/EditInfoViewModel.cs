namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Shared;
    using Base;
    using System.Collections.Generic;

    public class EditInfoViewModel : SettingItemViewModel
    {
        public EditInfoViewModel(ISearchAccountInfoRequest request, int displayOrder = 1)
            : base(request, displayOrder)

        {
        }

        protected override string BaseAddress => $"{OldMainUrl}_AccountInfo/EditCustomerInfo.aspx";

        public override Dictionary<string, string> QueryStringParamaters
            => new Dictionary<string, string>
            {
                ["custId"] = SearchAccount.Id.ToString(),
                ["username"] = SearchAccount.Username          
            };
    }
}