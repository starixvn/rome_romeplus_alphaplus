namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetHomeDrawAwayNoBetCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
    => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel121_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel121_MAg"
            : "Age_TotalBets_Sel121_Ag");
}