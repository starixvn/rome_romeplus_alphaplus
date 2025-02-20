using AlphaPlus.Core.Helpers;
using AlphaPlus.Products.Configurations;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base
{
    public class ProductStatusItemViewModel : StatusItemViewModel
    {
        private readonly ProductItem productItem;
        private const int DisplayOrderStep = 10;

        public ProductStatusItemViewModel(ISearchAccountInfoRequest request, ProductItem productItem)
            : base(request)
        {
            this.productItem = productItem;
        }

        public override string Text => Request.GetProductName(productItem.ProductId);

        public override int DisplayOrder => ProductId * DisplayOrderStep;

        public int ProductId => productItem.ProductId;

        public override string Url => MicroSites.SettingUrl("~/ProductStatus?" + string.Join("&",
                                          $"productid={productItem.ProductId}",
                                          $"custId={SearchAccount.Id}",
                                          $"custName={SearchAccount.Username}",
                                          $"custRoleId={SearchAccount.CustomerLevelValue}",
                                          $"agentId={SearchAccount.AgentId}",
                                          $"uid={Guid.NewGuid()}"));

        public override bool Disabled
            => base.Disabled ||
            Actor.IsNotDirectUplineOf(SearchAccount) ||
            SearchAccount.Upline.ProductStatusOf(productItem.ProductId).Disabled;

        public override bool Visible => Actor.IsDirectUplineOf(SearchAccount) && SearchAccount.CanSeeProduct(productItem.ProductId);

        public bool IsEditOnProductLevel => productItem.ExtendProductProperty.StatusOnProductLevel;

        public override bool Checked => SearchAccount.ProductStatusOf(productItem.ProductId).Enabled;
    }
}