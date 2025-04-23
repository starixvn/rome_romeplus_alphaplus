using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetNumberGameResultCriteria(
    DateTime fromDate,
    int sportType,
    int leagueId,
    string language,
    bool isShowAll) : CriteriaBase
{
    public DateTime FromDate { get; } = fromDate;

    public int SportType { get; } = sportType;

    public int LeagueId { get; } = leagueId;

    public string Language { get; } = language;

    public bool IsShowAll { get; } = isShowAll;

    public override string GetSettingKey()
        => "Age_Result_MatchSelNumberGameByLeague";

    public override bool IsValid()
        => FromDate > DateTime.MinValue && SportType > 0 && LeagueId >= 0;
}
