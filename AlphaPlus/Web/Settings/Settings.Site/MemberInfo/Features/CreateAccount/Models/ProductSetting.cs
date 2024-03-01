namespace AlphaPlus.Settings.Site.Features.CreateAccount.Models
{
    using System.Collections.Generic;
    public class ProductSetting
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IEnumerable<ProductSetting> Products { get; set; }
    }
}