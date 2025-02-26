namespace AlphaPlus.Products.Configurations
{
    public class ProductItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public bool Enabled { get; set; } = true;

        public DateTime EndSupportDate { get; set; } = DateTime.MaxValue;

        public ExtendProductProperty ExtendProductProperty { get; set; }
    }
}