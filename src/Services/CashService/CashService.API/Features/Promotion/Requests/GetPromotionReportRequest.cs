
namespace AlphaPlus.CashService.API.Features.Promotion.Requests;

public class GetPromotionReportRequest
{
    public int CustId { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public byte? PromotionType { get; init; }
    public bool? Status { get; init; }
}