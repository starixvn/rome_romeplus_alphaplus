namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ScoreMapResult(int row, int column) : Match
{
    public int RowNumber { get; init; } = row;

    public int ColumnNumber { get; init; } = column;

    public int[,] HaftTime { get; init; } = new int[row, column];

    public int[,] FullTime { get; init; } = new int[row, column];

    public new string Event => $"{HomeName} -vs- {AwayName}";
}
