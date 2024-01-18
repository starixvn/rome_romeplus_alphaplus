namespace AlphaPlus.Tickets.Site.Features.Shared.Requests;

public record RequestBase(
    int ViewerId = 23003140, //1176070
    byte ViewerLevel = 4,
    string Language = "en-US"
);
