namespace AlphaPlus.Reports.Features.EnteredResult;

public class EnteredResultSport(
    int sportTypeId,
    string sportName,
    EnteredResultEntity enteredResultToday,
    EnteredResultEntity enteredResultYesterday)
{
    public int SportTypeId { get; } = sportTypeId;

    public string SportName { get; } = sportName;

    public EnteredResultEntity EnteredResultToday { get; } = enteredResultToday;

    public EnteredResultEntity EnteredResultYesterday { get; } = enteredResultYesterday;
}