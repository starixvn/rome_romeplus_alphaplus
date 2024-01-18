namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetForecastRequest
{
    public int ViewerId { get; } = 23003140;

    public byte ViewerLevel { get; } = 4;

    public string Language { get; } = "en-US";

    public int LeagueId { get; set; }

    public bool Partial { get; init; }
}
