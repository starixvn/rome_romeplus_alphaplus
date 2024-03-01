namespace AlphaPlus.Settings.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class ProductMultiSelectionViewModel
    {
        public ProductMultiSelectionViewModel(
            IEnumerable<ProductViewModel> products,
            string userSelectedProductIds)
        {
            Products = products;
            UserSelectedProductIds = userSelectedProductIds;
        }

        public IEnumerable<ProductViewModel> Products { get; }

        public string UserSelectedProductIds { get; }
    }
}