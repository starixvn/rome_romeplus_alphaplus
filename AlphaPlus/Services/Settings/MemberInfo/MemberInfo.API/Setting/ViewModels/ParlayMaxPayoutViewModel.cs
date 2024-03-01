namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    using Base;
    using Requests;
    using System.Collections.Generic;

    public class ParlayMaxPayoutViewModel : SettingItemViewModel
    {
        public ParlayMaxPayoutViewModel(ISearchAccountInfoRequest request, int displayOrder = 4)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress => $"{MemberInfoUrl}ParlayMaxPayout/EditSingle";

        public override Dictionary<string, string> QueryStringParamaters
            => new Dictionary<string, string>
            {
                ["CustId"] = SearchAccount.Id.ToString(),
                ["CustName"] = SearchAccount.Username
            };

        public override string Text => Request.GetResource("ParlayMaxPayoutText");

        public string Title
            => Request.GetResource("ParlayMaxPayoutPopupHeaderText").Replace("<username>", SearchAccount.Username);
    }
}