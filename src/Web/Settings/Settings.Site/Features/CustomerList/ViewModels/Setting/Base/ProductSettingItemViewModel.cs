using AlphaPlus.Products.Configurations;
using AlphaPlus.Products.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base
{
    public abstract class ProductSettingItemViewModel(ISearchAccountInfoRequest request, ProductItem productItem)
        : SettingItemViewModel(request, productItem.ProductId * MaxSettingTypesOfAProduct)
    {
        private const int MaxSettingTypesOfAProduct = 5;
        protected readonly ISearchAccountInfoRequest request = request;
        protected readonly ProductItem productItem = productItem;
        protected readonly CustomerProductStatus customerStatus = request.SearchAccount.ProductStatusOf(productItem.ProductId);

        public int ProductId => productItem.ProductId;

        public override bool Enabled => customerStatus.Enabled;

        public override bool Visible => Actor.IsNotSuper || SearchAccount.IsNotAgent;

        public override string Icon
        {
            get
            {
                if (!request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission))
                {
                    return "icon-eye View";
                }

                return "icon-edit Enable";
            }
        }
    }
}