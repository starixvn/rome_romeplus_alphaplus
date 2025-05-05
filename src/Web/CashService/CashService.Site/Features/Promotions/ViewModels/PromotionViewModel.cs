using AlphaPlus.CashService.Site.Features.Promotions.Models;

namespace AlphaPlus.CashService.Site.Features.Promotions.ViewModels
{
    public class PromotionViewModel
    {
        public string Language { get; set; } = "en";

        public string DarkModeName { get; set; }

        public IEnumerable<PromotionModel> Promotion { get; set; }
    }
}
