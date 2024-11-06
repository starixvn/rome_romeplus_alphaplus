namespace AlphaPlus.Settings.Site.Features.CreateAccount.Models
{
    public class ProductSetting
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IEnumerable<ProductSetting> Products { get; set; }
    }
}