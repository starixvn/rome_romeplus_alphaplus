namespace AlphaPlus.CashService.Site.Features.Promotion.ViewModels;

public class GetPendingPromotionViewModel
{
    public int CustId { get; set; }
    public string Username { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public short StatusId { get; set; }
    public int? PromotionTypeId { get; set; }
    public int SiteDbGroupId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 5000;
}