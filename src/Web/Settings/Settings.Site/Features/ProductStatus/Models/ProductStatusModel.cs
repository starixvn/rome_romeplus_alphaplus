namespace AlphaPlus.Settings.Site.Features.ProductStatus.Models
{
    public class ProductStatusModel
    {
        public ProductStatusModel()
        {
        }

        public ProductStatusModel(
            int productId,
            int sportType,
            int betType,
            bool isDisabled,
            bool isDisabledUpline)
        {
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            IsDisabled = isDisabled;
            IsDisabledUpline = isDisabledUpline;
        }

        public int ProductId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsDisabledUpline { get; set; }

        public string Name { get; set; }

        public string TooltipMessage { get; set; }
    }
}