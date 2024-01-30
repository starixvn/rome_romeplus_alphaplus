namespace AlphaPlus.Tickets.Site.Features.Forecast.Requests;

public class GetSoccerResultRequest
{
    public int ViewerId { get; } = 23003140; //1176070

    public string Language { get; } = "en-US";

    public bool IsLive { get; set; } = true;

    public string MatchIds { get; set; } = "77819293,77819298";
}
