using AlphaPlus.CashService.Site.Features.Promotions.Models;

namespace AlphaPlus.CashService.Site.Features.Promotions.ViewModels
{
    public class PromotionViewModel
    {
        public string Language { get; set; } = "en";

        public IEnumerable<PromotionModel> Promotion { get; set; }
    }
}
