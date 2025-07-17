using AlphaPlus.Core.Extensions;

namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionDetails
{
    public string Username { get; init; }

    public string Nickname { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public DateTime FromDate { get; init; }

    public DateTime ToDate { get; init; }

    public string Period => $"{FromDate.ToShortDateWithSlash()} - {ToDate.ToShortDateWithSlash()}";

    public decimal Amount { get; init; }

    public string OperatedBy { get; init; }

    public DateTime OperatedTime { get; init; }

    public string LastUpdated => OperatedTime.ToLongDateTimeWithSlash();

    public string SubAccName { get; init; }

    public byte StatusId { get; init; }
}
