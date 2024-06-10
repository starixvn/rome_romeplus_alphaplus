namespace AlphaPlus.Tickets.Site.Features.BetLists.Models;

public static class TicketStatusIdConstants
{
    public const int Reject = 101;
    public const int Void = 102;
    public const int Refund = 103;

    public static readonly IEnumerable<int> All = [Reject, Void, Refund];
}