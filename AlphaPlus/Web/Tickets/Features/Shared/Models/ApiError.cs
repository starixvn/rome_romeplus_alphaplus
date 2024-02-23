namespace AlphaPlus.Tickets.Site.Features.Shared.Models;

public record ApiError(int Code, string Message, IDictionary<string, object> Data);
