using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Multiple
{
    public class ProductGroupMultipleSettingItemViewModel : ProductGroupSettingItemViewModel, ISettingItemMultipleViewModel
    {
        public ProductGroupMultipleSettingItemViewModel(ISearchAccountInfoRequest request, int productGroupId)
            : base(request, productGroupId)
        {
            switch (productGroupId)
            {
                case 1:
                    ShowGuideLine = true;
                    GuideLineUrl = $"{MicroSites.SettingUrl("~/CustomerList/GuideLine")}";
                    break;

                default:
                    ShowGuideLine = false;
                    GuideLineUrl = string.Empty;
                    break;
            }
        }

        public override Dictionary<string, string> QueryStringParameters
       => new()
       {
           ["productGroupId"] = ProductGroupId.ToString(),
           ["roleId"] = SearchAccount.CustomerLevelValue.ToString(),
           ["lng"] = request.Language,
           ["isDarkMode"] = request.IsDarkMode.ToString().ToLowerInvariant(),
           ["custIds"] = request.GetData<string>("customerIds"),
           ["usernames"] = request.GetData<string>("usernames"),
           ["agentId"] = SearchAccount.AgentId.ToString()
       };

        public override string Text => $"{request.GetResource("EditMultiple")} {request.GetProductGroupName(ProductGroupId)}";

        public override string Icon => "icon-edit Enable";

        protected override string BaseAddress => MicroSites.SettingUrl($"~/Customer/EditMultiple");

        public override string Url => BaseAddress;

        public override bool ShowGuideLine { get; }

        public string GuideLineUrl { get; }
    }
}