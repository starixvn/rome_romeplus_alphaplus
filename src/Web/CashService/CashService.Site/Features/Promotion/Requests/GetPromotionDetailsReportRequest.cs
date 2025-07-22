using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class GetPromotionDetailsReportRequest : BaseActionRequest
{
    public int PromotionId { get; init; }
    public string PromotionTitle { get; init; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; init; }
    public byte Status { get; init; }
    public string Username { get; set; }

    public bool IsValid()
    {
        FromDate = FromDate.GetGreaterDate(LimitedDateToShowInfo);
        return SqlDateTime.MinValue.Value < FromDate && FromDate <= ToDate && PromotionId > 0;
    }

    public override void SetUserSession(IUserSession userSession)
    {
        base.SetUserSession(userSession);
        SetLimitedDateToShowInfo(userSession);
        Username = userSession.IsSubAcc ? userSession.SubUserName : userSession.UserName;
    }

    public string ToQueryString()
    {
        var queryString = QueryString.Create([
            KeyValuePair.Create("custId", ActorId.ToString()),
            KeyValuePair.Create(nameof(FromDate), FromDate.ToShortDateWithSlash()),
            KeyValuePair.Create(nameof(ToDate), ToDate.ToShortDateWithSlash())
        ]);
        if (Status > 0)
        {
            queryString = queryString.Add(nameof(Status), Status.ToString());
        }

        return queryString.ToString();
    }
}
