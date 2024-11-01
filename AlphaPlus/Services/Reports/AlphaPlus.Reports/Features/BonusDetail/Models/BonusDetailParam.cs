namespace AlphaPlus.Reports.Features.BonusDetail;

public record BonusDetailParam(
    int ViewerId,
    byte ViewerLevelId,
    string Username,
    DateTime FromDate,
    DateTime ToDate,
    byte? StatusId,
    bool? IsComplained,
    int PageIndex,
    int PageSize);