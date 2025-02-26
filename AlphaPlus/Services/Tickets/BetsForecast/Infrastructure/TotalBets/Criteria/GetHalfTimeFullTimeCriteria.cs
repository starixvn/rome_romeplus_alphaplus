namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetHalfTimeFullTimeCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel16_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel16_MAg"
            : "Age_TotalBets_Sel16_Ag");
}