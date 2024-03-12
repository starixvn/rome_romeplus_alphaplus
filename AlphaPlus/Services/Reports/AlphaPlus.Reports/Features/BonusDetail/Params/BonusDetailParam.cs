namespace AlphaPlus.Reports.Features.BonusDetail;

public record BonusDetailParam(
    int ViewerId,
    UserLevel ViewerLevel,
    string Username,
    DateTime FromDate,
    DateTime ToDate,
    byte? StatusId,
    bool? IsComplained,
    int PageIndex,
    int PageSize);
