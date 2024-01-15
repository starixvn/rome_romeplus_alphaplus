namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ScoreMapResult(int row, int column, int[,] ht, int[,] ft) : Match
{
    public int RowNumber { get; } = row;

    public int ColumnNumber { get; } = column;

    public int[,] HaftTime { get; } = ht;

    public int[,] FullTime { get; } = ft;
}