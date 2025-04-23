namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetFirstGoalLastGoalCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel14_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel14_MAg"
            : "Age_TotalBets_Sel14_Ag");
}
