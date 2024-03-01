namespace AlphaPlus.Settings.MemberInfo.API.Status.ViewModels.Base
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Configuration;
    using Alpha.Products.Core.Shared;

    public class ProductStatusItemViewModel : StatusItemViewModel
    {
        private readonly IConfigurationItem productItem;
        private const int DisplayOrderStep = 10;

        public ProductStatusItemViewModel(ISearchAccountInfoRequest request, IConfigurationItem productItem)
            : base(request)
        {
            this.productItem = productItem;
        }

        public override string Text => Request.GetProductName(productItem.ProductId);

        public override int DisplayOrder => ProductId * DisplayOrderStep;

        public int ProductId => productItem.ProductId;

        public override string Url => AccountInfoUrl + "ProductStatus?" + string.Join("&",
                                          $"productid={productItem.ProductId}",
                                          $"custId={SearchAccount.Id}",
                                          $"custName={SearchAccount.Username}",
                                          $"custRoleId={SearchAccount.CusomerLevelValue}",
                                          $"uid={Guid.NewGuid()}");

        public override bool Disabled
            => base.Disabled || 
            Actor.IsNotDirectUplineOf(SearchAccount) || 
            SearchAccount.Upline.ProductStatusOf(productItem.ProductId).Disabled;

        public override bool Visible => Actor.IsDirectUplineOf(SearchAccount) && SearchAccount.CanSeeProduct(productItem.ProductId);

        public bool IsEditOnProductLevel
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            => (productItem.ExtendProductProperty as SearchAccountProperties).StatusOnProductLevel;

#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

        public override bool Checked => SearchAccount.ProductStatusOf(productItem.ProductId).Enabled;
    }
}