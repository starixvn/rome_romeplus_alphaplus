namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetScoreMapResultRequest
{
    public byte ViewerLevel { get; } = 4;

    public int ViewerId { get; } = 23003140; //1176070

    public string Language { get; } = "en-US";

    public string MatchIds { get; set; }

    public string LiveBetTypes { get; set; }

    public string DeadBetTypes { get; set; }
}