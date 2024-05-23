namespace AlphaPlus.Settings.Site.Features.CustomerList.Models
{
    using System.Collections.Generic;

    public class CustomerStatusModel(NonProductStatusModel nonProduct, IEnumerable<DCSProductStatusModel> dcsProducts)
    {
        public NonProductStatusModel NonProduct { get; } = nonProduct;

        public IEnumerable<DCSProductStatusModel> DCSProducts { get; } = dcsProducts;
    }
}