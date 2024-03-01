namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Configuration;
    using Base;
    using System.Collections.Generic;
    using static Alpha.Products.Core.Shared.ProductConstants;

    public class DynamicProductMultipleViewModel : DynamicProductViewModel, ISettingItemMultipleViewModel
    {
        private readonly bool isDarkMode;

        public DynamicProductMultipleViewModel(ISearchAccountInfoRequest request, IConfigurationItem productItem)
            : base(request, productItem)
        {
            switch (ProductId)
            {
                case LiveCasino:
                case Lottery:
                    ShowGuideLine = true;
                    GuideLineUrl = $"{MessageUrl}StaticMessage/Guide?id=135363";
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

        public override Dictionary<string, string> QueryStringParamaters
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
                    ["customerLevel"] = SearchAccount.CusomerLevelValue.ToString(),
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