namespace AlphaPlus.Products.Configurations
{
    public interface IExtendProductProperty
    {
    }

    public class ProductItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public bool Enabled { get; set; }

        public DateTime EndSupportDate { get; set; }

        public IExtendProductProperty ExtendProductProperty { get; set; }
    }
}