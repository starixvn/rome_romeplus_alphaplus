namespace AlphaPlus.Products.Models
{
    public interface ICustomerProductStatus
    {
        int ProductId { get; }
        bool Disabled { get; }
        bool Enabled { get; }
        bool Hidden { get; }
        bool CanBeSeen { get; }
    }

    public class CustomerProductStatus : ICustomerProductStatus
    {
        public CustomerProductStatus(
            int productId,
            bool disabled = false,
            bool hidden = false,
            int? productGroupId = null)
        {
            ProductId = productId;
            Disabled = disabled;
            Hidden = hidden;
            ProductGroupId = productGroupId;
        }

        public int ProductId { get; }
        public bool Disabled { get; }
        public bool Enabled => !Disabled;
        public bool Hidden { get; }
        public bool CanBeSeen => !Hidden;
        public int? ProductGroupId { get; }
    }
}