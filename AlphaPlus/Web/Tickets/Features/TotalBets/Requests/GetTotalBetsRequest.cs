namespace AlphaPlus.Tickets.Site.Features.TotalBets.Requests;

public record GetTotalBetsRequest(
    byte ViewerLevel = 4,
    int ViewerId = 23003140,//1176070
    string Language = "en-US",
    bool Partial = false);
