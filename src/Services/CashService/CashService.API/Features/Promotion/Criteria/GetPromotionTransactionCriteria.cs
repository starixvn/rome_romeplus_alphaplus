namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetPromotionTransactionCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int Status { get; set; }

    [SpParam(AllowNull = true)]
    public int? PromotionType { get; set; }

    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalRecords { get; set; } = 0;

    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashPromotion_Transaction_Sel";

    public override bool IsValid() => true;
}