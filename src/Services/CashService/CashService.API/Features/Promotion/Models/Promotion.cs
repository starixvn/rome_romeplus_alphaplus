using AlphaPlus.Shared.AppResources.CashServices;

namespace AlphaPlus.CashService.API.Features.Promotion.Models
{
    public class Promotion : BasePromotion
    {
        public string PromotionTitle { get; set; }
        public int PromotionTypeId { get; set; }

        public string PromotionTypeName => PromotionTypeId switch
        {
            1 => CashServicesResx.PromotionType_RebateOnTurnover,
            _ => "Unknown"
        };
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Enabled { get; set; }
        
        public List<PromotionProductSetting> ProductSettings { get; set; }
        public List<PromotionLanguageSetting> LanguageSettings { get; set; }
    }
}