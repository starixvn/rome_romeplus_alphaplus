namespace AlphaPlus.Reports.Features.BonusDetail;

public class GetSuperBonusCriteria(
    string username,
    DateTime fromDate,
    DateTime toDate,
    byte? statusId,
    bool? isComplained,
    int pageIndex,
    int pageSize,
    int superId) : GetBonusDetailCriteria(username, fromDate, toDate, statusId, isComplained, pageIndex, pageSize)
{
    public int SuperId { get; } = superId;

    public override string GetSettingKey() => "Age_Rpt_BonusDetail_Super";

    public override bool IsValid() => base.IsValid() && SuperId > 0;
}
