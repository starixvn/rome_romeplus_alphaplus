using AlphaPlus.Products.Configurations;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Products
{
    /// <summary>
    ///     View model for all products use DCS
    /// </summary>
    public class DynamicProductViewModel : ProductSettingItemViewModel
    {
        private readonly bool isDarkMode;

        public DynamicProductViewModel(ISearchAccountInfoRequest request, ProductItem productItem)
            : base(request, productItem)
        {
            isDarkMode = request.GetData<bool>(DataKey.Theme.IsDarkMode);
        }

        public override string Text => request.GetProductName(ProductId);

        protected override string BaseAddress => $"dcs/viewcustomersetting";

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