namespace AlphaPlus.CashService.API.Features.Promotion.Requests;

public class GetPromotionDetailsReportRequest
{
    public int CustId { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public byte Status { get; init; }
}