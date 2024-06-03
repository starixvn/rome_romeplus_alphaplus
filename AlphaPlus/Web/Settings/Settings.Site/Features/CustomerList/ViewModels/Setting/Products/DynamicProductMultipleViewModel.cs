using AlphaPlus.Products.Configurations;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using static AlphaPlus.Products.ProductConstants;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Products
{
    public class DynamicProductMultipleViewModel : DynamicProductViewModel, ISettingItemMultipleViewModel
    {
        private readonly bool isDarkMode;

        public DynamicProductMultipleViewModel(ISearchAccountInfoRequest request, ProductItem productItem)
            : base(request, productItem)
        {
            switch (ProductId)
            {
                case LiveCasino:
                case Lottery:
                    ShowGuideLine = true;
                    GuideLineUrl = MicroSites.FeatureUrl("~/StaticMessage/Guide?id=135363");
                    break;

                default:
                    ShowGuideLine = false;
                    GuideLineUrl = string.Empty;
                    break;
            }

            isDarkMode = request.GetData<bool>(DataKey.Theme.IsDarkMode);
        }

        public override string Text => $"{request.GetResource("EditMultiple")} {request.GetProductName(productItem.ProductId)}";

        public override string Icon => "icon-edit Enable";

        public override string Url => base.BaseAddress;

        public override string Method => DataKey.Method.POST;

        public override Dictionary<string, string> QueryStringParameters
        {
            get
            {
                var query = new Dictionary<string, string>
                {
                    ["isGetMultipleDCS"] = "1",
                    ["isMultiple"] = "true",
                    ["productId"] = ProductId.ToString(),
                    ["uplineId"] = Actor.Id.ToString(),
                    ["actorId"] = Actor.Id.ToString(),
                    ["customerIds"] = request.GetData<string>("customerIds"),
                    ["usernames"] = request.GetData<string>("usernames"),
                    ["customerNames"] = request.GetData<string>("usernames"),
                    ["customerLevel"] = SearchAccount.CustomerLevelValue.ToString(),
                    ["subAccId"] = Actor.SubAccountId.ToString(),
                    ["formType"] = "1",
                    ["isForcingMode"] = "false",
                    ["language"] = request.GetData<string>("language"),
                    ["isDarkMode"] = isDarkMode.ToString().ToLowerInvariant()
                };

                if (SearchAccount.IsMember)
                {
                    query.Add("uplinelevel", Actor.CustomerLevel.Value.ToString());
                }

                return query;
            }
        }

        public override bool ShowGuideLine { get; }

        public string GuideLineUrl { get; }
    }
}