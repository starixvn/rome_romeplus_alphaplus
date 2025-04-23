namespace AlphaPlus.Reports.Features.BonusDetail;

internal class GetMasterBonusCriteria(
    string username,
    DateTime fromDate,
    DateTime toDate,
    byte? statusId,
    bool? isComplained,
    int pageIndex,
    int pageSize,
    int masterId) : GetBonusDetailCriteria(username, fromDate, toDate, statusId, isComplained, pageIndex, pageSize)
{
    public int MasterId { get; } = masterId;

    public override string GetSettingKey() => "Age_Rpt_BonusDetail_Master";

    public override bool IsValid() => base.IsValid() && MasterId > 0;
}
