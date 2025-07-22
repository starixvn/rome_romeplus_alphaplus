using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

public class GetPromotionDetailsReportCriteria(
    int custId,
    int promotionId,
    DateTime fromDate,
    DateTime toDate,
    byte? statusId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int PromotionId { get; } = promotionId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public byte? StatusId { get; } = statusId;

    public override string GetSettingKey() => "Age_CashPromotion_PromotionDetailsReport";

    public override bool IsValid() => CustId > 0 && PromotionId > 0;
}
