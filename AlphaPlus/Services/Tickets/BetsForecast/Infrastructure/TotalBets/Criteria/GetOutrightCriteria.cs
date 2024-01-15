namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetOutrightCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel10_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel10_MAg"
            : "Age_TotalBets_Sel10_Ag");
}