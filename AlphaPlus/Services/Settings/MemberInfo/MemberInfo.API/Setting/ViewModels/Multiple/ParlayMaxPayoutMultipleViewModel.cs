namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using Base;
    using Requests;
    using System.Collections.Generic;

    public class ParlayMaxPayoutMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        public ParlayMaxPayoutMultipleViewModel(ISearchAccountInfoRequest request, int displayOrder = 1004)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress => $"{MemberInfoUrl}ParlayMaxPayout/EditMultiple";

        public override string Url => BaseAddress;

        public override Dictionary<string, string> QueryStringParamaters
            => new Dictionary<string, string>
            {
                ["CustIds"] = Request.GetData<string>("customerIds"),
                ["CustNames"] = Request.GetData<string>("usernames")
            };

        public override string Text => Request.GetResource("ParlayMaxPayoutMultipleText");

        public override string Method => DataKey.Method.POST;
    }
}