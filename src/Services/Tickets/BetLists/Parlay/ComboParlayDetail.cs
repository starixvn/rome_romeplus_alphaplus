namespace AlphaPlus.BetLists.Parlay;

public class ComboParlayDetail
{
    public DateTime WinLostDate { get; init; }

    public long TransId { get; init; }

    public long Serial { get; init; }

    public int StatusId { get; init; }

    public string Status { get; init; }

    public decimal MasterOdds { get; init; }

    public decimal Odds { get; init; }

    public decimal Stake { get; init; }

    public decimal WinLost { get; init; }

    public decimal PlayerComm { get; init; }

    public decimal ExtraValue { get; init; }

    public string ExtraInfo { get; init; }
}
