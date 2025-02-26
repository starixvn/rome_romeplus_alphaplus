namespace AlphaPlus.Settings.Site.Features.ProductStatus.Models
{
    public class ProductStatusUM
    {
        public ProductStatusUM()
        {
        }

        public ProductStatusUM(
            int productId,
            int sportType,
            int betType,
            byte settingType,
            DateTime beginTime,
            DateTime endTime)
        {
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            SettingType = settingType;
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public int ProductId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public byte SettingType { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}