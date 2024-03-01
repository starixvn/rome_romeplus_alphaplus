namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Configuration;
    using Base;

    /// <summary>
    ///     View model for all products use DCS
    /// </summary>
    public class DynamicProductViewModel : ProductSettingItemViewModel
    {
        private readonly bool isDarkMode;

        public DynamicProductViewModel(ISearchAccountInfoRequest request, IConfigurationItem productItem)
            : base(request, productItem)
        {
            isDarkMode = request.GetData<bool>(DataKey.Theme.IsDarkMode);
        }

        public override string Text => request.GetProductName(ProductId);

        protected override string BaseAddress
            => $"{OldMainUrl}dcs/viewcustomersetting";

        public override Dictionary<string, string> QueryStringParamaters
        {
            get
            {
                var query = new Dictionary<string, string>
                {
                    ["customerLevel"] = SearchAccount.CustomerLevel.Value.ToString(),
                    ["customerId"] = SearchAccount.Id.ToString(),
                    ["productId"] = ProductId.ToString(),
                    ["isDarkMode"] = isDarkMode.ToString().ToLowerInvariant()
                };

                if (SearchAccount.IsMember)
                {
                    query.Add("uplinelevel", Actor.CustomerLevel.Value.ToString());
                }

                return query;
            }
        }

        public override bool Visible => base.Visible && Actor.CanSeeProduct(ProductId);
    }
}