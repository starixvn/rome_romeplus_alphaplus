using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.MemberRequest
{
    public class ProductsViewModel
    {
        public ProductsViewModel(
            int viewId,
            int selectedProductId,
            IEnumerable<ProductViewModel> products)
        {
            ViewerId = viewId;
            Products = products;
            SelectedProductId = selectedProductId;
        }

        public int ViewerId { get; }

        public int SelectedProductId { get; }

        public IEnumerable<ProductViewModel> Products { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
           new ProductMultiSelectionViewModel(Products, SelectedProductId.ToString());
    }
}