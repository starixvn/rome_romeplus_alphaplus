namespace AlphaPlus.Tickets.Site.Features.TotalBets.Requests;

public class GetTotalBetsRequest
{
    public byte ViewerLevel { get; } = 4;

    public int ViewerId { get; } = 23003140; //1176070

    public string Language { get; } = "en-US";

    public bool Partial { get; set; }
}