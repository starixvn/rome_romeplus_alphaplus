namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetTotalGoalCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel6_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel6_MAg"
            : "Age_TotalBets_Sel6_Ag");
}