using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class GetPromotionReportRequest : BaseActionRequest
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; init; }
    public byte PromotionType { get; init; }
    public byte Status { get; init; }

    public bool IsValid()
    {
        FromDate = FromDate.GetGreaterDate(LimitedDateToShowInfo);
        return SqlDateTime.MinValue.Value < FromDate && FromDate <= ToDate;
    }

    public string ToQueryString()
        => QueryString.Create(new[]
        {
            KeyValuePair.Create("custId", ActorId.ToString()),
            KeyValuePair.Create(nameof(FromDate), FromDate.ToShortDateWithSlash()),
            KeyValuePair.Create(nameof(ToDate), ToDate.ToShortDateWithSlash()),
            KeyValuePair.Create(nameof(PromotionType), PromotionType.ToString()),
            KeyValuePair.Create(nameof(Status), Status.ToString())
        }).ToString();
}