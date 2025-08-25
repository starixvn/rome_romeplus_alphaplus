namespace AlphaPlus.CashService.API.Features.Promotion.Models
{
    public class Promotion : BasePromotion
    {
        public string PromotionTitle { get; set; }
        public int PromotionTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Enabled { get; set; }
        public List<PromotionProductSetting> ProductSettings { get; set; }
        public List<PromotionLanguageSetting> LanguageSettings { get; set; }
    }
}