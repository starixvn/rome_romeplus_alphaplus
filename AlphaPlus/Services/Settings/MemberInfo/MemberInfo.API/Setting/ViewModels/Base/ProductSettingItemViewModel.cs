namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Configuration;
    using Alpha.Products.Core.Models;

    public abstract class ProductSettingItemViewModel : SettingItemViewModel
    {
        private const int MaxSettingTypesOfAProduct = 5;
        protected readonly ISearchAccountInfoRequest request;
        protected readonly IConfigurationItem productItem;
        protected readonly CustomerProductStatus customerStatus;

        protected ProductSettingItemViewModel(ISearchAccountInfoRequest request, IConfigurationItem productItem)
            : base(request, productItem.ProductId * MaxSettingTypesOfAProduct)
        {
            this.productItem = productItem;
            this.request = request;
            this.customerStatus = request.SearchAccount.ProductStatusOf(productItem.ProductId);
        }

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