using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class GetPromotionReportRequest : BaseActionRequest
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; init; }
    public byte PromotionType { get; init; }
    public byte Status { get; init; }
    public string Username { get; set; }

    public bool IsValid()
    {
        FromDate = FromDate.GetGreaterDate(LimitedDateToShowInfo);
        return SqlDateTime.MinValue.Value < FromDate && FromDate <= ToDate;
    }

    public override void SetUserSession(IUserSession userSession)
    {
        base.SetUserSession(userSession);
        SetLimitedDateToShowInfo(userSession);
        Username = userSession.IsSubAcc ? userSession.SubUserName : userSession.UserName;
    }

    public string ToQueryString()
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custId", ActorId.ToString()),
            KeyValuePair.Create(nameof(FromDate), FromDate.ToShortDateWithSlash()),
            KeyValuePair.Create(nameof(ToDate), ToDate.ToShortDateWithSlash())
        });
        if (PromotionType != 0) // All
        {
            queryString = queryString.Add(nameof(PromotionType), PromotionType.ToString());
        }
        if (Status != 2) // All
        {
            queryString = queryString.Add(nameof(Status), Status.ToString());
        }

        return queryString.ToString();
    }
}