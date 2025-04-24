using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Outright;

public class GetOutRightResultCriteria(
    DateTime fromDate,
    DateTime toDate,
    bool isShowAll) : CriteriaBase
{
    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public bool IsShowAll { get; } = isShowAll;

    public override string GetSettingKey() => "Age_Result_Outright";

    public override bool IsValid()
        => DateTime.MinValue < FromDate && FromDate <= ToDate;
}