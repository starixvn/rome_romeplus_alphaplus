using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class GetManualBonusRequest : BaseActionRequest
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; init; }
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
        Username = userSession.UserName;
    }

    public string ToQueryString()
    {
        var queryString = QueryString.Create([
            KeyValuePair.Create("custId", ActorId.ToString()),
            KeyValuePair.Create(nameof(FromDate), FromDate.ToShortDateWithSlash()),
            KeyValuePair.Create(nameof(ToDate), ToDate.ToShortDateWithSlash())
        ]);

        return queryString.ToString();
    }
}