using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BonusDetail;

internal abstract class GetBonusDetailCriteria(
    string username,
    DateTime fromDate,
    DateTime toDate,
    byte? statusId,
    bool? isComplained,
    int pageIndex,
    int pageSize) : CriteriaBase
{
    public string Username { get; } = username;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public byte? StatusId { get; } = statusId;

    public bool? IsComplained { get; } = isComplained;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRecords { get; set; }

    public override bool IsValid() => FromDate <= ToDate;
}
