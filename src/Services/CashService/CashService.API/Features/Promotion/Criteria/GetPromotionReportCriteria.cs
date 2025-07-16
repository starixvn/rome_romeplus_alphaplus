using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

public class GetPromotionReportCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    byte? promotionType,
    bool? statusId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public byte? PromotionType { get; } = promotionType;

    public bool? StatusId { get; } = statusId;

    public override string GetSettingKey() => "Age_CashPromotion_PromotionReport";

    public override bool IsValid() => CustId > 0;
}
