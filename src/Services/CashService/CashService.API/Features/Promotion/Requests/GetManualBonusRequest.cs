namespace AlphaPlus.CashService.API.Features.Promotion.Requests;

public class GetManualBonusRequest
{
    public int CustId { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
}