namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetOE1X2DNDCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel25_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel25_MAg"
            : "Age_TotalBets_Sel25_Ag");
}