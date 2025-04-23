namespace AlphaPlus.Reports.Features.EnteredResult;

public record EnteredResultSport(
    int SportTypeId,
    string SportName,
    EnteredResultEntity EnteredResultToday,
    EnteredResultEntity EnteredResultYesterday);