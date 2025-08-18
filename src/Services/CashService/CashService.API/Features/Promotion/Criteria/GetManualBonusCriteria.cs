using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

public class GetManualBonusCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public override string GetSettingKey() => "Age_CashPromotion_ManualBonus_Sel";

    public override bool IsValid() => CustId > 0;
}