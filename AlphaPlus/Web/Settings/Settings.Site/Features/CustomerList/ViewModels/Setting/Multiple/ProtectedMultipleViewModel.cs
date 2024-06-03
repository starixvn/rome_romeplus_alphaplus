using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Multiple
{
    public class ProtectedMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        private new const int DisplayOrder = 1003;

        public ProtectedMultipleViewModel(ISearchAccountInfoRequest request) : base(request, DisplayOrder)
        {
        }

        public override string Text => Request.GetResource("EditMultipleProtectedStatusText");

        protected override string BaseAddress => MicroSites.SettingUrl("~/ProtectedStatus/EditMultiple");

        public override string Url => BaseAddress;

        public override Dictionary<string, string> QueryStringParameters
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
