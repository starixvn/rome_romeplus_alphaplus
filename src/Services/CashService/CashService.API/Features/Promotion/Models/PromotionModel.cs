namespace AlphaPlus.CashService.API.Features.Promotion.Models
{
    public class PromotionModel
    {
        public int PromotionId { get; set; }
        public string PromotionTitle { get; set; }
        public int PromotionTypeId { get; set; }
        public int PromotionType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Enabled { get; set; }
        public List<PromotionProductSetting> ProductSettings { get; set; }
        public List<Product> Products { get; set; }
        public int StatusId { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}