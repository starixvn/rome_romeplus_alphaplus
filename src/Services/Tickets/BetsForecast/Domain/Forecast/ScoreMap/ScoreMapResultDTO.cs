namespace AlphaPlus.BetsForecast.Domain.Forecast;

public class ScoreMapResultDTO(IEnumerable<ScoreMapResultEntity> scoreMapResults) : Match
{
    public IEnumerable<ScoreMapResultEntity> ScoreMapResults { get; } = scoreMapResults;
}