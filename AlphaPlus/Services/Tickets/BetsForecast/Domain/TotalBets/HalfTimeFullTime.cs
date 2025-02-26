namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class HalfTimeFullTime : TotalBetsMatch
{
    public decimal TotalHH { get; init; }

    public decimal TotalHD { get; init; }

    public decimal TotalHA { get; init; }

    public decimal TotalDH { get; init; }

    public decimal TotalDD { get; init; }

    public decimal TotalDA { get; init; }

    public decimal TotalAH { get; init; }

    public decimal TotalAD { get; init; }

    public decimal TotalAA { get; init; }
}