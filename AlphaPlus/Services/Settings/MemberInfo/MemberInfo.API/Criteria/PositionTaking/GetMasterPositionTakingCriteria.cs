namespace AlphaPlus.Settings.MemberInfo.API.Criteria;

public class GetMasterPositionTakingCriteria : GetPositionTakingCriteriaBase
{
    public int SRecommend { get; set; }

    public override bool IsValid() => SRecommend > 0;
}