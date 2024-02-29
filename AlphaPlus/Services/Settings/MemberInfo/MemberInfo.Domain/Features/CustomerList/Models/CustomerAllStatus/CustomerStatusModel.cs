namespace AlphaPlus.Settings.MemberInfo.Domain.Features.CustomerList.Models
{
    using System.Collections.Generic;

    public class CustomerStatusModel
    {
        public CustomerStatusModel(
            NonProductStatusModel nonProduct,
            NonDCSProductStatusModel nonDCSProduct,
            IEnumerable<DCSProductStatusModel> dcsProducts)
        {
            NonProduct = nonProduct;
            NonDCSProduct = nonDCSProduct;
            DCSProducts = dcsProducts;
        }

        public NonProductStatusModel NonProduct { get; }

        public NonDCSProductStatusModel NonDCSProduct { get; }

        public IEnumerable<DCSProductStatusModel> DCSProducts { get; }
    }
}