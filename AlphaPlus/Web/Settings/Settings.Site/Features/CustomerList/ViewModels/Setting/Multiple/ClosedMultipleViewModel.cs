using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Multiple
{
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

        protected override string BaseAddress => MicroSites.SettingUrl("~/OpenDownlines/EditMultiple");

        public override string Url => BaseAddress;

        public override Dictionary<string, string> QueryStringParameters
            => new()
            {
                ["CustIds"] = Request.GetData<string>("customerIds"),
                ["RoleId"] = SearchAccount.CustomerLevelValue.ToString()
            };

        public override bool Visible => !(request.Actor.IsSuper && request.SearchAccount.IsAgent);
    }
}