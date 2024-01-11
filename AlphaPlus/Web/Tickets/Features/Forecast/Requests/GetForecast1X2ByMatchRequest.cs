namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetForecast1X2ByMatchRequest
{
    public byte ViewerLevel { get; } = 4;

    public int ViewerId { get; } = 23003140; //1176070

    public string Language { get; } = "en-US";

    public bool Partial { get; set; }

    public int MatchId { get; set; }

    public int BetType { get; set; }
}