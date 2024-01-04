namespace AlphaPlus.Tickets.Site.Features.TotalBets.Requests;

public class GetTotalBetsRequest
{
    public byte ViewerLevel { get; } = 4;

    public int ViewerId { get; } = 23003140;

    public string Language { get; } = "en";

    public bool Partial { get; set; }
}