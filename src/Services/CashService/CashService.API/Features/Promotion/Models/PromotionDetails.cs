using System.Text.Json.Serialization;

namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionDetails
{
    public string Username { get; init; }

    public string Nickname { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string PromotionType { get; init; }

    [JsonIgnore]
    public DateTime FromDate { get; init; }

    [JsonIgnore]
    public DateTime ToDate { get; init; }

    public string DateRange
        => $"{FromDate.ToString("MM/dd/yyyy")} - {ToDate.ToString("MM/dd/yyyy")}";

    public decimal BonusAmount { get; init; }

    public DateTime LastUpdated { get; init; }

    public byte Status { get; init; }
}