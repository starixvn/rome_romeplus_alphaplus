namespace AlphaPlus.CashService.API.Features.Promotion.Requests;

public class GetPromotionTransactionRequest
{
    public int CustId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int StatusId { get; set; }
    public int? PromotionTypeId { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int SiteDbGroupId { get; set; }
}