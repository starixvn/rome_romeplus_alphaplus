namespace AlphaPlus.BetLists.CancelledBets;

public static class TicketStatusConstants
{
    public const int Reject = 101;
    public const int Void = 102;
    public const int Refund = 103;
    public static readonly IEnumerable<int> All = [Void, Reject, Refund];
}
